using CRUD_The_First.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CRUD_The_First.Services;
public class BufferedFileUploadLocalService:IBufferedFileUploadService
{
    private readonly IWebHostEnvironment _environment;
    private readonly ApplicationContext _context;
    public BufferedFileUploadLocalService(IWebHostEnvironment environment, ApplicationContext context)
    {
        _environment = environment;
        _context = context;

    }
    public async Task<(bool,string)> UploadFile(IFormFile file)
    {
        var lastId = _context.Files.Max(x=>x.Id);
        var idString = $"{lastId}".PadLeft(8, '0') ;
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
                
                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file.FileName);
                var fileExt = Path.GetExtension(file.FileName);
                var fileNameWithId = fileNameWithoutExt +"_"+ idString + fileExt;
                var destFilePath = Path.Combine(path, fileNameWithId);

                Debug.WriteLine($"Dest file: {destFilePath}");
                
                using (var fileStream = new FileStream(destFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                var url = destFilePath.Replace(Environment.CurrentDirectory, "").Replace(@"\wwwroot","");
                return (true,url);
            }
            else
            {
                return (false,"");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception: {ex}");
            throw new Exception("File Copy Failed", ex);
        }
    }
}
