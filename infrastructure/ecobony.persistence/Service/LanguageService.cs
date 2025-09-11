using Microsoft.Extensions.Caching.Distributed;

namespace ecobony.persistence.Service;

public class LanguageService(
    IDistributedCache _cache,
    ILanguageReadRepository _languageRead,
    ILanguageWriteRepository _languageWrite,
    ILanguageJsonService languageJsonService,
    IHttpContextAccessor _contextAccessor,
    IFileService _fileService,
    IMemoryCache _memoryCache
    ):ILanguageService
{
    public async Task<List<Language>> GetCLientAll()
    {

        var latestUpdateAt = await _languageRead.GetFilter(x => !x.isDeleted)
         .MaxAsync(x => (DateTime?)(
             x.UpdateAt > x.CreateAt ? x.UpdateAt : x.CreateAt
         ));



        string cacheKey = "languages_cache";

        // Cache-də varsa və hələ expired deyil → qaytar
        if (_memoryCache.TryGetValue(cacheKey, out List<Language>? cachedLanguages))
        {
            // Əgər cacheExpiry artıq keçibsə → DB-dən yenilə
            if (latestUpdateAt <= DateTime.UtcNow.ToLocalTime())
            {
                cachedLanguages = await _languageRead.GetAll().ToListAsync();

                // Yeni məlumatı cache-ə əlavə et, expiration 1 dəqiqə kimi qoya bilərsən
                _memoryCache.Set(cacheKey, cachedLanguages, TimeSpan.FromMinutes(10));
            }

            return cachedLanguages;
        }

        // Cache-də yoxdursa → DB-dən götür və cache-ə əlavə et
        var languages =  await _languageRead.GetAll().ToListAsync(); ;
        _memoryCache.Set(cacheKey, languages, TimeSpan.FromMinutes(10));
        return languages;

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
            throw new BadRequestException(languageJsonService.LanguageStrongJson("InvalidGuid"));

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
            throw new BadRequestException(languageJsonService.LanguageStrongJson("ReplatValuesName"));
        if (await _languageRead.AnyAsync(a => a.Key == model.IsoCode  &&  a.Name==model.IsoCode && a.IsoCode==model.IsoCode))
            throw new BadRequestException(languageJsonService.LanguageStrongJson("ReplatValuesIsoCode"));

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
            throw new BadRequestException(languageJsonService.LanguageStrongJson("InvalidGuid"));
        var language = await _languageRead.GetByIdAsync(model.Id) 
            ?? throw new NotFoundException(languageJsonService.LanguageStrongJson("NotFound"));

        if (model.FormFile is not null)
        { if(!string.IsNullOrEmpty(language.Image)) throw new NotFoundException(languageJsonService.LanguageStrongJson("ImageNotFound"));
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

        _contextAccessor?.HttpContext?.Response.Cookies.Append("Language", language.IsoCode);

       
    }

    public async Task<bool> SoftDelete(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException(languageJsonService.LanguageStrongJson("InvalidGuid"));
        var language = await _languageRead.GetSingleAsync(a=>a.isDeleted==false) ?? throw new NotFoundException(languageJsonService.LanguageStrongJson("NotFound"));

        language.isDeleted = true;
        _languageWrite.Update(language);
        await _languageWrite.SaveChangegesAsync();
        return true;
    }

    public async Task<bool> Restore(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException(languageJsonService.LanguageStrongJson("InvalidGuid"));
        var language = await _languageRead.GetSingleAsync(a=>a.isDeleted==true)
            ?? throw new NotFoundException(languageJsonService.LanguageStrongJson("NotFound"));

        language.isDeleted=false;
        _languageWrite.Update(language);
        await _languageWrite.SaveChangegesAsync();
        return true;
    }

    public async Task<bool> Delete(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException(languageJsonService.LanguageStrongJson("InvalidGuid"));
        var language = await _languageRead.GetSingleAsync(a=>a.isDeleted==true)
            ?? throw new NotFoundException(languageJsonService.LanguageStrongJson("NotFound"));
        if(string.IsNullOrEmpty(language.Image)) throw new NotFoundException(languageJsonService.LanguageStrongJson("ImageNotFound"));
        _fileService.DeleteFile(language.Image);
        _languageWrite.Delete(language);
        await _languageWrite.SaveChangegesAsync();
        return true;
    }
}