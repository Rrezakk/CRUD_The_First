namespace CRUD_The_First.Models;

public class FIleEditingModel
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string? Link { get; set; }
    public FIleEditingModel(FileModel fileModel)
    {
        this.Id = fileModel.Id;
        this.Link = fileModel.StaticUrl;
        this.FileName = fileModel.FileName;
    }
}
