namespace ecobony.application.Services;

public interface ILanguageService
{
    Task<List<Language>> GetCLientAll();
    Task<List<Language>> GetAdminAll();
    Task<Language> GetById(string id);
    Task<bool> Create(CreateLanguageDto_s model);
    Task<bool> Update(UpdateLanguageDto_s model);

    Task GetByIsoCodeAsync(string isoCode);

    Task<bool> SoftDelete(string id);
    Task<bool> Restore(string id);
    Task<bool> Delete(string id);
}