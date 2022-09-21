using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class GamingStoreContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Device> Devices => Set<Device>();

    public GamingStoreContext(DbContextOptions<GamingStoreContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.SeedAdmin();
    }
}
