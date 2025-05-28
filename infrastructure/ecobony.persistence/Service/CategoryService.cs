using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace ecobony.persistence.Service;

public class CategoryService(
    ICategoryTranslationWriteRepository categoryTranslationWrite,
    ICategoryTranslationReadRepository categoryTranslationRead,
    ICategoryReadRepository _categoryRead,
    ICategoryWriteRepository _categoryWrite,
    IHttpContextAccessor _contextAccessor,
    ILanguageReadRepository _languageRead,
    IMemoryCache _memoryCache,
    IFileService _fileService
    
    ):ICategoryService
{
    public async Task<List<GetCategoryDto_s>> GetClientAll()
    {
       
        var languageCode = _contextAccessor?.HttpContext?.Session?.GetString("Language");

        var language = await _languageRead.GetSingleAsync(a => a.IsoCode == languageCode)
                      
                       ?? throw new NotFoundException();

        var latestUpdateAt = await _categoryRead.GetFilter
            (x => !x.isDeleted)
            .MaxAsync(x =>
                (DateTime?)(
                    (x.UpdateAt > x.CreateAt ? x.UpdateAt : x.CreateAt))
            );

        string cacheKey = $"categories_{language.Id}";
        if (_memoryCache.TryGetValue(cacheKey, out (DateTime lastUpdate, List<GetCategoryDto_s> data) cachedData))
        {
            if (cachedData.lastUpdate>=latestUpdateAt)
            {
                return cachedData.data; 
            }
        }
        var category = await _categoryRead.GetAll()
            .Include(a=>a.Wastes)
            .Select(a => new
            {
                a.Id,
                a.Count,
                a.UpdateAt,
                a.isDeleted,
                a.Image,
                a.CreateAt,
                a.Wastes,
                a.Pointy,
                Translation = a.CategoryTranslations.FirstOrDefault(t => t.LanguageId == language.Id )
            })
            
            .Where(a => a.Translation != null && a.isDeleted==false)
            .Select(a => new GetCategoryDto_s
            {
                CategoryId = a.Id,
                Count = a.Wastes.Where(w=>w.CategoryId==a.Id).Count(),
                CreateAt = a.CreateAt,
                UpdateAt = a.UpdateAt,
                isDeleted = a.isDeleted,
                Pointy = a.Pointy,
                Image = a.Image,
                CategoryTranslationId = a.Translation.Id,
                LanguageId = a.Translation.LanguageId,
                Name = a.Translation.Name,
                Description = a.Translation.Description
            })
            .ToListAsync();

        
        _memoryCache.Set(cacheKey, (latestUpdateAt, category), new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });
        return category;
    }


    public async Task<List<GetCategoryDto_s>> GetAdminAll()
    
    {
        var latestUpdateAt = await _categoryRead.GetFilter
                (x => !x.isDeleted)
            .MaxAsync(x =>
                (DateTime?)(
                    (x.UpdateAt > x.CreateAt ? x.UpdateAt : x.CreateAt))
            );

        string cacheKey = $"categories_{latestUpdateAt}";
        if (_memoryCache.TryGetValue(cacheKey, out (DateTime lastUpdate, List<GetCategoryDto_s> data) cachedData))
        {
            if (cachedData.lastUpdate>=latestUpdateAt)
            {
                return cachedData.data; 
            }
        }
        var category = await _categoryRead.GetAll()
            .Include(a=>a.Wastes)
            .Select(a => new
            {
                a.Id,
                a.Count,
                a.CreateAt,
                a.Image,
                a.UpdateAt,
                a.isDeleted,
                a.Wastes,
                a.Pointy,
                Translation = a.CategoryTranslations.FirstOrDefault()
            })
            .Where(a => a.Translation != null )
            .Select(a => new GetCategoryDto_s
            {
                CategoryId = a.Id,
                Pointy = a.Pointy,
                Count = a.Wastes.Where(w=>w.CategoryId==a.Id).Count(),
             
                CreateAt = a.CreateAt,
                isDeleted = a.isDeleted,
                UpdateAt = a.UpdateAt,
                Image = a.Image,
                CategoryTranslationId = a.Translation.Id,
                LanguageId = a.Translation.LanguageId,
                Name = a.Translation.Name,
                Description = a.Translation.Description
            })
            .ToListAsync();

        
    
        _memoryCache.Set(cacheKey, (latestUpdateAt, category), new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });
        return category;
    }

    public async Task<GetCategoryDto_s> GetById(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException($"Invalid GUID format: '{id}'");
        var languageCode = _contextAccessor.HttpContext.Session.GetString("Language");

        var language = await _languageRead.GetSingleAsync(a => a.IsoCode == languageCode)
                       ?? throw new NotFoundException();
        var category = await _categoryRead.GetByIdAsync(id) ?? throw new NotFoundException();

        var translation =
            await categoryTranslationRead.GetSingleAsync(a =>
                a.CategoryId == category.Id && a.LanguageId == language.Id && a.isDeleted==false);

        return new GetCategoryDto_s
        {
            CategoryId = category.Id,
            Count = category.Wastes.Where(w=>w.CategoryId==category.Id).Count(),
            Pointy = category.Pointy,
            isDeleted = category.isDeleted,
            CreateAt = category.CreateAt,
            UpdateAt = category.UpdateAt,
            CategoryTranslationId = translation.Id,
            LanguageId = translation.LanguageId,
            Name = translation.Name,
            Description = translation.Description
        };
    }

    public async Task<bool> Create(CreateCategoryWithTranslationDto model)
    {
        var image = await _fileService.UploadFileAsync(model.CreateCategoryDto.FormFile);

        if (image is null) throw new NotFoundException();

        if (await _categoryRead.AnyAsync(a => a.Pointy == model.CreateCategoryDto.Pointy))
            throw new BadRequestException("Replat values point ");
        
        Category category = new Category()
        {
            Pointy = model.CreateCategoryDto.Pointy,
            PricePoint = model.CreateCategoryDto.PricePoint,
            Image = image
        };
        
        await _categoryWrite.AddAsync(category);
        await _categoryWrite.SaveChangegesAsync();

        var languges = await _languageRead.GetByIdAsync(model.CreateCategoryTranslationDto.LanguageId.ToString()) ?? throw new NotFoundException();
        if (await categoryTranslationRead
                .AnyAsync(a => a.Name == model.CreateCategoryTranslationDto.Name && a.LanguageId==languges.Id))
            throw new BadRequestException("Replat values name ");
        CategoryTranslation categoryTranslation = new CategoryTranslation()
        {
            Name = model.CreateCategoryTranslationDto.Name,
            Description = model.CreateCategoryTranslationDto.Description,
            LanguageId = languges.Id,
            CategoryId = category.Id,
        };
        await categoryTranslationWrite.AddAsync(categoryTranslation);
        await categoryTranslationWrite.SaveChangegesAsync();
        return true;
    }

    public async Task<bool> Update(UpdateCategoryWithTranslationDto model)
    {
        if (!Guid.TryParse(model.UpdateCategoryDto.Id, out _))
            throw new BadRequestException($"Invalid GUID format: '{model.UpdateCategoryDto.Id}'");

        
        var category = await _categoryRead.GetByIdAsync(model.UpdateCategoryDto.Id) ?? throw new NotFoundException();

        if (model.UpdateCategoryDto.FormFile is not null)
        {

            _fileService.DeleteFile(category.Image);
            var image = await _fileService.UploadFileAsync(model.UpdateCategoryDto.FormFile);

            if (image is null) throw new NotFoundException();

            category.Image = image;
            
        }
        category.Pointy = model.UpdateCategoryDto.Pointy;
        category.PricePoint = model.UpdateCategoryDto.PricePoint;

         _categoryWrite.Update(category);
         await _categoryWrite.SaveChangegesAsync();


         var languages = await _languageRead.GetByIdAsync(model.UpdateCategoryTranslationDto.LanguageId.ToString()) ??
                         throw new NotFoundException();
         var translation = await categoryTranslationRead.GetSingleAsync(a => a.CategoryId == category.Id) ??
                           throw new NotFoundException();
         translation.CategoryId = category.Id;
         translation.Description = model.UpdateCategoryTranslationDto.Description;
         translation.Name = model.UpdateCategoryTranslationDto.Name;
         translation.LanguageId = languages.Id;

         return true;

    }

    public async Task<bool> SoftDelete(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException($"Invalid GUID format: '{id}'");

        var category = await _categoryRead.GetSingleAsync(a=>a.Id==Guid.Parse(id) && a.isDeleted==false) ?? throw new NotFoundException();

        category.isDeleted =true;
        _categoryWrite.Update(category);
        await _categoryWrite.SaveChangegesAsync();
        var translation = await categoryTranslationRead.GetSingleAsync(a => a.CategoryId == category.Id && a.isDeleted==false) ??
                          throw new NotFoundException();
        translation.isDeleted = true;
        categoryTranslationWrite.Update(translation);
        await categoryTranslationWrite.SaveChangegesAsync();
        return true;
    }

    public async Task<bool> Restore(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException($"Invalid GUID format: '{id}'");

        var category = await _categoryRead.GetSingleAsync(a=>a.Id==Guid.Parse(id) && a.isDeleted==true) ?? throw new NotFoundException();

        category.isDeleted = false;
        _categoryWrite.Update(category);
        await _categoryWrite.SaveChangegesAsync();
        var translation = await categoryTranslationRead.GetSingleAsync(a => a.CategoryId == category.Id && a.isDeleted==true) ??
                          throw new NotFoundException();
        translation.isDeleted=false;
        categoryTranslationWrite.Update(translation);
        await categoryTranslationWrite.SaveChangegesAsync();
        return true;
    }

    public async Task<bool> Delete(string id)
    { 
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException($"Invalid GUID format: '{id}'");

        var category = await _categoryRead.GetSingleAsync(a=>a.Id==Guid.Parse(id) && a.isDeleted==true) ?? throw new NotFoundException();
        _fileService.DeleteFile(category.Image);
        _categoryWrite.Update(category);
        await _categoryWrite.SaveChangegesAsync();
        return true;
    }
}