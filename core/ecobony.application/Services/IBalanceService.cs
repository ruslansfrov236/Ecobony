namespace ecobony.application.Services
{
    public interface IBalanceService
    {

        Task<List<BalanceTransfer>> GetAdminAll();
        Task<List<BalanceTransfer>> GetClientAll();
        Task<BalanceTransfer> GetById(string id);
        Task<bool> Create(string bonusId, decimal bonus);
        Task<bool> SoftDelete(string id);
        Task<bool> RestoreDelete(string id);
        Task<bool> Delete(string id);
    }
}