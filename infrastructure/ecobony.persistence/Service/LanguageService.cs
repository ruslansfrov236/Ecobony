namespace ecobony.persistence.Service;

public class LanguageService(
    ILanguageReadRepository _languageRead,
    ILanguageWriteRepository _languageWrite,
    IHttpContextAccessor _contextAccessor,
    IFileService _fileService,
    IMemoryCache _memoryCache
    ):ILanguageService
{
    public async Task<List<Language>> GetCLientAll()
    {
        var latestUpdateAt = await _languageRead.GetFilter
                (x => !x.isDeleted)
            .MaxAsync(x =>
                (DateTime?)(
                    (x.UpdateAt > x.CreateAt ? x.UpdateAt : x.CreateAt))
            );

        string cacheKey = $"languages_{latestUpdateAt}";
        if (_memoryCache.TryGetValue(cacheKey, out (DateTime lastUpdate, List<Language> data) cachedData))
        {
            if (cachedData.lastUpdate>=latestUpdateAt)
            {
                return cachedData.data; 
            }
        }
        var language = await _languageRead.GetFilter(a => a.isDeleted == false).ToListAsync();
        
        _memoryCache.Set(cacheKey, (latestUpdateAt, language), new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });

        return language;
    }



    public async Task<List<Language>> GetAdminAll()
    {
        var latestUpdateAt = await _languageRead.GetFilter
                (x => !x.isDeleted)
            .MaxAsync(x =>
                (DateTime?)(
                    (x.UpdateAt > x.CreateAt ? x.UpdateAt : x.CreateAt))
            );

        string cacheKey = $"languages_{latestUpdateAt}";
        if (_memoryCache.TryGetValue(cacheKey, out (DateTime lastUpdate, List<Language> data) cachedData))
        {
            if (cachedData.lastUpdate>=latestUpdateAt)
            {
                return cachedData.data; 
            }
        }
        var languages =await _languageRead.GetAll().ToListAsync();
        _memoryCache.Set(cacheKey, (latestUpdateAt, languages), new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });

        return languages;
    } 

    public async Task<Language> GetById(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException($"Invalid GUID format: '{id}'");

        var latestUpdateAt = await _languageRead.GetFilter
                (x => !x.isDeleted)
            .MaxAsync(x =>
                (DateTime?)(
                    (x.UpdateAt > x.CreateAt ? x.UpdateAt : x.CreateAt))
            );

        string cacheKey = $"languages_{id}";
        if (_memoryCache.TryGetValue(cacheKey, out (DateTime lastUpdate, Language data) cachedData))
        {
            if (cachedData.lastUpdate>=latestUpdateAt)
            {
                return cachedData.data; 
            }
        }
        var languages =await _languageRead.GetByIdAsync(id);
        _memoryCache.Set(cacheKey, (latestUpdateAt, languages), new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });

        return languages;
    }

    public async Task<bool> Create(CreateLanguageDto_s model)
    {
        if (await _languageRead.AnyAsync(a => a.Key == model.Key &&  a.Name==model.Key && a.IsoCode==model.Key))
            throw new BadRequestException("Replat values Key ");


        if (await _languageRead.AnyAsync(a => a.Key == model.Name &&  a.Name==model.Key && a.IsoCode==model.Key))
            throw new BadRequestException("Replat values Name ");
        if (await _languageRead.AnyAsync(a => a.Key == model.IsoCode  &&  a.Name==model.IsoCode && a.IsoCode==model.IsoCode))
            throw new BadRequestException("Replat values IsoCode ");

        var image = await _fileService.UploadFileAsync(model.FormFile);

        Language language = new Language()
        {
            Name = model.Name,
            IsoCode = model.IsoCode,
            Key = model.Key,
            Image = image
        };
        await _languageWrite.AddAsync(language);
        await _languageWrite.SaveChangegesAsync();
        return true;
    }

    public async Task<bool> Update(UpdateLanguageDto_s model)
    {
        if (!Guid.TryParse(model.Id, out _))
            throw new BadRequestException($"Invalid GUID format: '{model.Id}'");
        var language = await _languageRead.GetByIdAsync(model.Id) ?? throw new NotFoundException();

        if (model.FormFile is not null)
        { 
            _fileService.DeleteFile(language.Image);
            var image = await _fileService.UploadFileAsync(model.FormFile);

            language.Image = image;
        }

        language.IsoCode = model.IsoCode;
        language.Key = model.Key;
        language.Name = model.Name;

        _languageWrite.Update(language);
        await _languageWrite.SaveChangegesAsync();
        return true;
    }

    public async Task GetByIsoCodeAsync(string isoCode)
    {
        var language = await _languageRead
            .GetSingleAsync(a => a.IsoCode == isoCode) ?? throw new NotFoundException();

        _contextAccessor.HttpContext.Session.SetString("Language", language.IsoCode);

       
    }

    public async Task<bool> SoftDelete(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException($"Invalid GUID format: '{id}'");
        var language = await _languageRead.GetSingleAsync(a=>a.isDeleted==false) ?? throw new NotFoundException();

        language.isDeleted = true;
        _languageWrite.Update(language);
        await _languageWrite.SaveChangegesAsync();
        return true;
    }

    public async Task<bool> Restore(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException($"Invalid GUID format: '{id}'");
        var language = await _languageRead.GetSingleAsync(a=>a.isDeleted==true) ?? throw new NotFoundException();

        language.isDeleted=false;
        _languageWrite.Update(language);
        await _languageWrite.SaveChangegesAsync();
        return true;
    }

    public async Task<bool> Delete(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException($"Invalid GUID format: '{id}'");
        var language = await _languageRead.GetSingleAsync(a=>a.isDeleted==true) ?? throw new NotFoundException();

     
        _languageWrite.Delete(language);
        await _languageWrite.SaveChangegesAsync();
        return true;
    }
}