using Domain.Entities;
using Infrastructure.ConfigurationModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public class GamingStoreContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    private readonly AdminData _adminData = new();

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Device> Devices => Set<Device>();

    public GamingStoreContext(
        DbContextOptions<GamingStoreContext> options,
        IConfiguration configuration)
        : base(options)
    {
        configuration.GetSection("Admin").Bind(_adminData);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.SeedRoles();
        modelBuilder.SeedAdmin(_adminData);
    }
}
