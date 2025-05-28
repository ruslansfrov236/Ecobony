namespace ecobony.application.Services;

public interface ICategoryService
{
    Task<List<GetCategoryDto_s>> GetClientAll();
    Task<List<GetCategoryDto_s>> GetAdminAll();
    Task<GetCategoryDto_s> GetById(string id);
    Task<bool> Create(CreateCategoryWithTranslationDto model);
    Task<bool> Update(UpdateCategoryWithTranslationDto model);
    Task<bool> SoftDelete(string id);
    Task<bool> Restore(string id);
    Task<bool> Delete(string id);
}