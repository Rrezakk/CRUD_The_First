using CRUD_The_First.Extensions;
using CRUD_The_First.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_The_First.Data;

public sealed class ApplicationContext:DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyUtcDateTimeConverter();//Put before seed data and after model creation
    }
    public DbSet<FileModel> Files { get; set; }
}
