namespace ecobony.application.Services;

public interface ILocationService
{
       Task<List<Location>> GetClientAll();
    Task<List<Location>> GetAdminAll();
    Task<Location> GetById(string id);
    Task<bool> Create(CreateLocationDto_s model);
    Task<bool> Update(UpdateLocationDto_s model);
    Task<bool> SoftDelete(string id);
    Task<bool> Restore(string id);
    Task<bool> Delete(string id);

}