namespace ecobony.application.Services;

public interface IWasteService
{
   
    Task<List<Waste>> GetClientAll();
    Task<List<Waste>> GetAdminAll();
    Task<Waste> GetById(string id);
    Task<List<Waste>> GetWasteTrash();
    Task<List<Waste>> GetFilterWasteCategory(string categoryId);
    Task<bool> Create(CreateWasteDto_s model);
    Task<bool> Update(UpdateWasteDto_s model);
    Task<bool> SoftDelete(string id);
    Task<bool> Restore(string id);
    Task<bool> Delete(string id);
}