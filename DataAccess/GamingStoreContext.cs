using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess;

public class GamingStoreContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    private readonly string _adminPassword;

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Device> Devices => Set<Device>();

    public GamingStoreContext(
        DbContextOptions<GamingStoreContext> options,
        IConfiguration configuration)
        : base(options)
    {
        _adminPassword = configuration.GetSection("AdminSettings:Password").Value;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.SeedRoles();
        modelBuilder.SeedAdmin(_adminPassword);
    }
}
