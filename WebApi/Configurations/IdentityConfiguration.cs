using Core.Entities;
using DataAccess;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Configurations;

public static class IdentityConfiguration
{
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<int>>()
            .AddEntityFrameworkStores<GamingStoreContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
        });
    }
}
