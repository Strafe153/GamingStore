using Domain.Entities;
using IdentityServer.Configurations;
using IdentityServer.Services;
using IdentityServer4.Services;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.ServiceExtensions;

public static class ServiceInstaller
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        string dbConnectionString = configuration.GetConnectionString("DatabaseConnection");
        var assembly = typeof(Program).Assembly.GetName().Name;

        services.AddDbContext<GamingStoreContext>(options => options.UseSqlServer(dbConnectionString));

        services.AddIdentity<User, IdentityRole<int>>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
        })
        .AddEntityFrameworkStores<GamingStoreContext>()
        .AddDefaultTokenProviders();

        services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseSqlServer(
                    dbConnectionString, options => options.MigrationsAssembly(assembly));
            })
            .AddInMemoryIdentityResources(IdentityServerConfiguration.GetIdentityResources())
            .AddInMemoryApiResources(IdentityServerConfiguration.GetApiResources())
            .AddInMemoryApiScopes(IdentityServerConfiguration.GetApiScopes())
            .AddInMemoryClients(IdentityServerConfiguration.GetClients())
            .AddAspNetIdentity<User>();

        services.AddScoped<IProfileService, ProfileService>();
    }
}
