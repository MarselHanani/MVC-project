namespace Booky.PL.Helper;

public static class FileManagement
{
    public static string Upload(IFormFile file, string  folderName)
    {
        if (file == null)
        {
            return null;
        }
        string folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\files", folderName);
        string fileName = $"{Guid.NewGuid()}{file.FileName}";
        
        string filePath = Path.Combine(folderPath, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream); 
            return fileName;
    }
}