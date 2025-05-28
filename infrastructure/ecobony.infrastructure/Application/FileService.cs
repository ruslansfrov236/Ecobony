namespace ecobony.infrastructur.Application;

public class FileService(
    IWebHostEnvironment _environment
    
    ):IFileService
{
    public async Task<string> UploadFileAsync(IFormFile formFile)
    {
        var imageDirectory = Path.Combine(_environment.WebRootPath, "assets", "img");
        var tempDirectory = Path.Combine(_environment.WebRootPath, "assets", "temp");

        // Ensure the image and temp directories exist
        if (!Directory.Exists(imageDirectory))
            Directory.CreateDirectory(imageDirectory);

        if (!Directory.Exists(tempDirectory))
            Directory.CreateDirectory(tempDirectory);

       var existingFiles = Directory.GetFiles(imageDirectory, "image-*.png")
            .Select(p => Path.GetFileNameWithoutExtension(p))        
            .Select(p => p.Replace("image-", ""))                     
            .Where(p => int.TryParse(p, out _))                        
            .Select(int.Parse)                                        
            .ToList();

       
        int newNumber = existingFiles.Any() ? existingFiles.Max() + 1 : 1;


        var fileName = $"image-{newNumber}.png";
        var tempPath = Path.Combine(tempDirectory, fileName);
        var imagePath = Path.Combine(imageDirectory, fileName);

        // Save the file to the temp directory
        using (var fileStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
        {
            await formFile.CopyToAsync(fileStream);
        }

        // Process the image (e.g., resizing, encoding)
        using (var image = await Image.LoadAsync(tempPath))
        {
            await image.SaveAsync(imagePath, new PngEncoder());
        }
        // Clean up the temporary file
        File.Delete(tempPath);

        return fileName;
    }
    

    public async Task<List<string>> UploadMultipleFilesAsync(IFormCollection formCollection)
    {
        var imageDirectory = Path.Combine(_environment.WebRootPath, "assets", "img");
        var tempDirectory = Path.Combine(_environment.WebRootPath, "assets", "temp");

        if (!Directory.Exists(imageDirectory))
            Directory.CreateDirectory(imageDirectory);

        if (!Directory.Exists(tempDirectory))
            Directory.CreateDirectory(tempDirectory);

        var existingFiles = Directory.GetFiles(imageDirectory, "image-*.png")
            .Select(p => Path.GetFileNameWithoutExtension(p))
            .Select(p => p.Replace("image-", ""))
            .Where(p => int.TryParse(p, out _))
            .Select(int.Parse)
            .ToList();

        int nextNumber = existingFiles.Any() ? existingFiles.Max() + 1 : 1;

        var uploadedFileNames = new List<string>();

        foreach (var formFile in formCollection.Files)
        {
            if (formFile.Length > 0)
            {
                var fileName = $"image-{nextNumber}.png";
                var tempPath = Path.Combine(tempDirectory, fileName);
                var imagePath = Path.Combine(imageDirectory, fileName);

                // Faylı müvəqqəti qovluğa yaz
                using (var fileStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
                {
                    await formFile.CopyToAsync(fileStream);
                }

                // Şəkili emal et və img qovluğuna yaz
                using (var image = await Image.LoadAsync(tempPath))
                {
                    await image.SaveAsync(imagePath, new PngEncoder());
                }

                // Müvəqqəti faylı sil
                File.Delete(tempPath);

                uploadedFileNames.Add(fileName);
                nextNumber++;
            }
        }

        return uploadedFileNames;
    }

    public void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);
    }

}