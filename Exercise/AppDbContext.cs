// Data/AppDbContext.cs

using Code.Model;
using Microsoft.EntityFrameworkCore;

namespace Code;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("Server=localhost,1433;User Id=sa;Password=Code@123;Encrypt=False;TrustServerCertificate=True;");
    }
}