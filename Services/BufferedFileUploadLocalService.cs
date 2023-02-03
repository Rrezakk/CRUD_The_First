namespace CRUD_The_First.Services;
public class BufferedFileUploadLocalService:IBufferedFileUploadService
{
    private IWebHostEnvironment _environment;
    public BufferedFileUploadLocalService(IWebHostEnvironment environment)
    {
        _environment = environment;

    }
    public async Task<(bool,string)> UploadFile(IFormFile file)
    {
        string path = "";
        try
        {
            if (file.Length > 0)
            {
                path = Path.GetFullPath(Path.Combine(_environment.WebRootPath, "UploadedFiles"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var destFilePath = Path.Combine(path, file.FileName);
                using (var fileStream = new FileStream(destFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return (true,destFilePath);
            }
            else
            {
                return (false,"");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("File Copy Failed", ex);
        }
    }
}
