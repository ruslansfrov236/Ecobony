
namespace ecobony.application
{
    public interface IUserHistoryService
    {
        // CRUD əməliyyatları
        Task<List<UserHistory>> GetAdminAsync();
        Task<UserHistory> GetClientAll();
        Task<bool> Create();
        Task<bool> SoftDelete(string id);
        Task<bool> Delete(string id);
        Task<bool> RestoreDelete(string id);

        // Filtrləmə və axtarış
        Task<List<UserHistory>> GetByUserIdAsync(string userId);
        Task<List<UserHistory>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<UserHistory>> GetByActionTypeAsync(ActionType actionType);
        Task<List<UserHistory>> SearchAsync(string keyword);

        // Pagination və Sorting
        Task<PagedResult<UserHistory>> GetPagedAsync(int pageNumber, int pageSize, string sortBy, bool isDescending);

        // Statistik hesabatlar
        Task<int> GetTotalActionCountAsync();
        
        Task<Dictionary<ActionType, int>> GetActionStatisticsAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<DateTime, int>> GetDailyActivityAsync(DateTime startDate, DateTime endDate);

        // Təhlükəsizlik və audit
        
        Task<List<UserHistory>> GetSuspiciousActivitiesAsync(DateTime startDate, DateTime endDate);
        Task<bool> LogUserActionAsync(LogUserActionDto_s model);

       
    }

}
