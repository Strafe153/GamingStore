using Microsoft.EntityFrameworkCore;
using GamingDevicesStore.Models;

namespace GamingDevicesStore.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Device> Devices => Set<Device>();

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
