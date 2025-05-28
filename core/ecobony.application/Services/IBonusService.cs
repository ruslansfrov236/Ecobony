
namespace ecobony.application.Services;

public interface IBonusService
{
    Task<List<BonusComunity>> GetAdminAll();
    Task<List<BonusComunity>> GetClientAll();
    Task<FileStreamResult> GetClientExcelView(DateTime? StartDate , DateTime? EndDate);
    Task<FileStreamResult> GetClinetPdfView(DateTime? StartDate , DateTime? EndDate);
    Task<bool> Create(string wasteId);
    Task<bool> SoftDelete(string id);
    Task<bool> RestoreDelete(string id);
    Task<bool> Delete(string id);
}