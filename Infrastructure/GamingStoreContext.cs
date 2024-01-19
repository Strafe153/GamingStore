using Domain.Entities;
using Infrastructure.Configurations.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public class GamingStoreContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    private readonly AdminOptions _adminOptions = new();

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Device> Devices => Set<Device>();

    public GamingStoreContext(
        DbContextOptions<GamingStoreContext> options,
        IOptions<AdminOptions> adminOptions)
        : base(options)
    {
        _adminOptions = adminOptions.Value;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.SeedRoles();
        modelBuilder.SeedAdmin(_adminOptions);
    }
}
