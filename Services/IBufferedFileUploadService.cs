namespace CRUD_The_First.Services;

public interface IBufferedFileUploadService
{
    Task<(bool,string)> UploadFile(IFormFile file);
}
