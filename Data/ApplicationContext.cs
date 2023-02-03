using CRUD_The_First.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_The_First.Data;

public sealed class ApplicationContext:DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
    {
    }
    public DbSet<FIleModel> Files { get; set; }
}
