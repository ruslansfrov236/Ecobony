
namespace ecobony.application.Services;

public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file);
    Task<List<string>> UploadMultipleFilesAsync(IFormCollection formCollection);
    void DeleteFile(string filePath);
}