namespace ecobony.application.Services;

public interface IHeaderService
{
    Task<List<GetHeadetDto>> GetClientAll();
    Task<List<GetHeadetDto>> GetAdminAll();
    Task<GetHeadetDto> GetById(string id);
    Task<bool> Create(CreateHeaderWithTranslationDto_s model);
    Task<bool> Update(UpdateHeaderWithTranslationDto_s model);
    Task<bool> SoftDelete(string id);
    Task<bool> Restore(string id);
    Task<bool> Delete(string id);

}