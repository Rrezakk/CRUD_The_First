using System.ComponentModel.DataAnnotations;

namespace CRUD_The_First.Models;

public class FIleModel
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string FileName { get; set; }
    [Required]
    public string StaticUrl { get; set; }
    public DateTime CreationTime { get; set; } = DateTime.Now;
}
