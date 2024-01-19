using Domain.Shared.Constants;
using Infrastructure;
using Infrastructure.Configurations.Models;
using Microsoft.EntityFrameworkCore;

namespace Web.Configurations;

public static class DatabaseConfiguration
{
    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GamingStoreContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(ConnectionStringsConstants.DatabaseConnection)!));

        services.Configure<AdminOptions>(configuration.GetSection(AdminOptions.SectionName));
    }
}
