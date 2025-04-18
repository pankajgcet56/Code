// Data/AppDbContext.cs

using Code.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Code;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    private readonly IConfiguration _configuration;

    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        options.UseSqlServer(connectionString);
    }
}