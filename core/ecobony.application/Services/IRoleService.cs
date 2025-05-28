
namespace ecobony.application.Services;

public interface IRoleService
{
    Task<List<AppRole>> GetAll();
    Task<AppRole> GetById(string id);
    Task<bool> Create(CreateRolesDto_s model);
    Task<bool> Update(UpdateRolesDto_s model);
    Task<bool> Delete(string id);
}