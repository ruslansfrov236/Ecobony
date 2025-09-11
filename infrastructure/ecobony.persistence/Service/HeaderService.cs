using Microsoft.AspNetCore.Http;

namespace ecobony.persistence.Service;

public class HeaderService(
    IHeaderReadRepository _headerRead,
    IHeaderWriteRepository _headerWrite,
    UserManager<AppUser> _userManager,
    IHeaderTranslationReadRepository _headerTranslationRead,
    IHeaderTranslationWriteRepository _headerTranslationWrite,
    ILanguageReadRepository _languageRead,
    ILanguageJsonService _languageJsonService,
    IHttpContextAccessor _contextAccessor,
    IMemoryCache _memoryCache,
    IFileService _fileService
    
    ):IHeaderService
{
    public async Task<List<GetHeadetDto>> GetClientAll()
    {
        var languageCode = CultureInfo.CurrentCulture.Name; 

        var language = await _languageRead.GetSingleAsync(a => a.IsoCode == languageCode && a.isDeleted==false)
                       ?? throw new NotFoundException();

        
        var latestUpdateAt = await _headerRead.GetFilter
                (x => !x.isDeleted)
            .MaxAsync(x =>
                (DateTime?)(
                    (x.UpdateAt > x.CreateAt ? x.UpdateAt : x.CreateAt))
            );

        string cacheKey = $"header{language.IsoCode}";
        if (_memoryCache.TryGetValue(cacheKey, out (DateTime lastUpdate, List<GetHeadetDto> data) cachedData))
        {
            if (cachedData.lastUpdate>=latestUpdateAt)
            {
                return cachedData.data; 
            }
        }
        var header = await _headerRead.GetAll().Select(a => new
        {
            a.Image,
            a.Role,
            a.Id,
            a.CreateAt,
            a.UpdateAt,
            a.isDeleted,
           Translation= a.HeaderTranslations.FirstOrDefault(h => h.HeaderId == a.Id)
        }).Where(a=>a.Translation != null && a.Translation.LanguageId==language.Id).Select(
            t=> new GetHeadetDto()
            {
                Id = t.Translation.Id.ToString(),
                Image = t.Image,
                CreateAt = t.CreateAt,
                UpdateAt = t.UpdateAt,
                isDeleted = t.isDeleted,
                Title = t.Translation.Title,
                Description = t.Translation.Description,
                LanguageId= language.Id,
                
                HeaderId = t.Id,
                
            }).ToListAsync();

        _memoryCache.Set(cacheKey, (latestUpdateAt, header), new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });
        return header;
    }

    public async Task<List<GetHeadetDto>> GetAdminAll()
    {
     
        var latestUpdateAt = await _headerRead.GetFilter
                (x => !x.isDeleted)
            .MaxAsync(x =>
                (DateTime?)(
                    (x.UpdateAt > x.CreateAt ? x.UpdateAt : x.CreateAt))
            );

        string cacheKey = $"header{latestUpdateAt}";
        if (_memoryCache.TryGetValue(cacheKey, out (DateTime lastUpdate, List<GetHeadetDto> data) cachedData))
        {
            if (cachedData.lastUpdate>=latestUpdateAt)
            {
                return cachedData.data; 
            }
        }
        var header = await _headerRead.GetAll().Select(a => new
        {
            a.Image,
            a.Role,
            a.Id,
            a.CreateAt,
            a.UpdateAt,
            a.isDeleted,
            Translation= a.HeaderTranslations.FirstOrDefault(h => h.HeaderId == a.Id)
        }).Where(a=>a.Translation != null ).Select(
            t=> new GetHeadetDto()
            {
                Id = t.Translation.Id.ToString(),
                
                Image = t.Image,
                CreateAt = t.CreateAt,
                UpdateAt = t.UpdateAt,
                isDeleted = t.isDeleted,
                Title = t.Translation.Title,
                Description = t.Translation.Description,
                LanguageId= t.Translation.LanguageId,
                HeaderId = t.Id,
                
            }).ToListAsync();
        _memoryCache.Set(cacheKey, (latestUpdateAt, header), new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });
        return header;
    }

    public async Task<GetHeadetDto> GetById(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException(_languageJsonService.LanguageStrongJson("InvalidGuid"));
        var languageCode = CultureInfo.CurrentCulture.Name; 

        var language = await _languageRead.GetSingleAsync(a => a.IsoCode == languageCode && a.isDeleted==false)
                       ?? throw new NotFoundException();

        var username = _contextAccessor?.HttpContext?.User?.Identity?.Name
              ?? throw new CustomUnauthorizedException(_languageJsonService.LanguageStrongJson("Unauthorized")); ;
        AppUser user = await _userManager.FindByNameAsync(username)
                       ?? throw new CustomUnauthorizedException(_languageJsonService.LanguageStrongJson("UserNotFound"));

        var userRoles = await _userManager.GetRolesAsync(user) 
            ?? throw new NotFoundException(_languageJsonService.LanguageStrongJson("RoleNotFound"));

        Header header = null;
        HeaderTranslation translation = null;
        foreach (var role in userRoles)
        {
            if (role == RoleModel.Admin.ToString() || role == RoleModel.Manager.ToString())
            {
                header = await _headerRead.GetByIdAsync(id);


                translation = await _headerTranslationRead
                   .GetSingleAsync(a => a.HeaderId == header.Id && a.LanguageId == language.Id);
                break;
            }
            else if (role == RoleModel.User.ToString())
            {
                header = await _headerRead.GetSingleAsync(a => a.Id == Guid.Parse(id) && a.isDeleted == false);


                translation = await _headerTranslationRead
                   .GetSingleAsync(a => a.HeaderId == header.Id && a.LanguageId == language.Id && a.isDeleted == false);
                break;
            }
        }
            if(header is null || translation is null )
          throw new NotFoundException(_languageJsonService.LanguageStrongJson("NotFound"));



            return new GetHeadetDto()
        {

            Id = translation.Id.ToString(),
            Image = header.Image,
            CreateAt = header.CreateAt,
            UpdateAt = header.UpdateAt,
            isDeleted = header.isDeleted,
            Title = translation.Title,
            Description = translation.Description,
            LanguageId = language.Id,
            HeaderId = header.Id,
        };
    }

    public async Task<bool> Create(CreateHeaderWithTranslationDto_s model)
    {
        var image = await _fileService.UploadFileAsync(model.CreateHeaderDto.FomFile);
        Header header = new Header()
        {
            Image = image,
            Role = model.CreateHeaderDto.Role
        };
        await _headerWrite.AddAsync(header);
        await _headerWrite.SaveChangegesAsync();

        if (model.CreateHeaderTranslationDto.Title != null || model.CreateHeaderTranslationDto.Description != null)
        {
            var languges = await _languageRead.GetByIdAsync(model.CreateHeaderTranslationDto.LanguageId.ToString()) ?? throw new NotFoundException();
     
            if(await _headerTranslationRead.
                   AnyAsync(a=>a.Title==model.CreateHeaderTranslationDto.Title 
                               && a.LanguageId==languges.Id))     throw new BadRequestException("Replat values title ");
             
            if(await _headerTranslationRead.
                   AnyAsync(a=>a.Description==model.CreateHeaderTranslationDto.Description 
                               && a.LanguageId==languges.Id))     throw new BadRequestException("Replat values description ");

            HeaderTranslation headerTranslation = new HeaderTranslation()
            {
                Title = model.CreateHeaderTranslationDto.Title,
                Description = model.CreateHeaderTranslationDto.Description,
                LanguageId = languges.Id
            };
        }
        return true;
    }

    public async Task<bool> Update(UpdateHeaderWithTranslationDto_s model)
    {
        if(Guid.TryParse(model.UpdateHeaderDto.Id.ToString(), out _))
            throw new BadRequestException(_languageJsonService.LanguageStrongJson("InvalidGuid"));
        var header = await _headerRead.GetByIdAsync(model.UpdateHeaderDto.Id)
            ?? throw new NotFoundException(_languageJsonService.LanguageStrongJson("NotFound"));

        if (model.UpdateHeaderDto.FomFile != null)
        {
            _fileService.DeleteFile(header.Image);
            var image = await _fileService.UploadFileAsync(model.UpdateHeaderDto.FomFile);
            header.Image = image;
        }

        header.Role = model.UpdateHeaderDto.Role;
        _headerWrite.Update(header);
        await _headerWrite.SaveChangegesAsync();
        if (model.UpdateHeaderTranslationDto.Title != null || model.UpdateHeaderTranslationDto.Description != null)
        {
            var languges = await _languageRead.GetByIdAsync(model.UpdateHeaderTranslationDto.LanguageId.ToString()) ??
                           throw new NotFoundException();
            var translation = await _headerTranslationRead.GetSingleAsync(a=>a.HeaderId==header.Id && a.LanguageId==languges.Id)
                ??  throw new NotFoundException();
            translation.Title = model.UpdateHeaderTranslationDto.Title;
            translation.Description = model.UpdateHeaderTranslationDto.Description;
            translation.LanguageId = languges.Id;
            translation.HeaderId = header.Id;

            _headerTranslationWrite.Update(translation);
            await _headerTranslationWrite.SaveChangegesAsync();
            
        }

        return true;
    }

    public async Task<bool> SoftDelete(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException(_languageJsonService.LanguageStrongJson("InvalidGuid"));
        var languageCode = CultureInfo.CurrentCulture.Name;

        var language = await _languageRead.GetSingleAsync(a => a.IsoCode == languageCode && a.isDeleted==false)
                       ?? throw new NotFoundException(_languageJsonService.LanguageStrongJson("NotFound"));
        var header = await _headerRead.GetSingleAsync(a=>a.Id==Guid.Parse(id) && a.isDeleted==false) 
                     ?? throw new NotFoundException(_languageJsonService.LanguageStrongJson("NotFound"));
        header.isDeleted = true;
        _headerWrite.Update(header);
        await _headerWrite.SaveChangegesAsync();
        var translation = await _headerTranslationRead
                              .GetSingleAsync(a=>a.HeaderId==header.Id && a.isDeleted==false)
                          ?? throw new NotFoundException(_languageJsonService.LanguageStrongJson("NotFound"));
        translation.isDeleted = true;
        _headerTranslationWrite.Update(translation);
        await _headerTranslationWrite.SaveChangegesAsync();
        return true;
    }

    public async Task<bool> Restore(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException(_languageJsonService.LanguageStrongJson("InvalidGuid"));
        var languageCode =CultureInfo.CurrentCulture.Name;

        var language = await _languageRead.GetSingleAsync(a => a.IsoCode == languageCode)
                       ?? throw new NotFoundException(_languageJsonService.LanguageStrongJson("NotFound"));
        var header = await _headerRead.GetSingleAsync(a=>a.Id==Guid.Parse(id) && a.isDeleted==true) 
                     ?? throw new NotFoundException(_languageJsonService.LanguageStrongJson("NotFound"));
        header.isDeleted=false;
        _headerWrite.Update(header);
        await _headerWrite.SaveChangegesAsync();
        var translation = await _headerTranslationRead
                              .GetSingleAsync(a=>a.HeaderId==header.Id && a.isDeleted==true)
                          ?? throw new NotFoundException(_languageJsonService.LanguageStrongJson("NotFound"));
        translation.isDeleted=false;
        _headerTranslationWrite.Update(translation);
        await _headerTranslationWrite.SaveChangegesAsync();
        return true;
    }

    public async Task<bool> Delete(string id)
    {
        var header = await _headerRead.GetSingleAsync(a=>a.Id==Guid.Parse(id) && a.isDeleted==true) 
                     ?? throw new NotFoundException(_languageJsonService.LanguageStrongJson("NotFound"));
        _fileService.DeleteFile(header.Image);
        _headerWrite.Delete(header);
        await _headerWrite.SaveChangegesAsync();
        return true;
    }
}