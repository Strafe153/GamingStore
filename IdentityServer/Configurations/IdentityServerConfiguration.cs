using Domain.Entities;
using Domain.Shared.Constants;
using IdentityModel;
using IdentityServer.Services;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityConstants = Domain.Shared.Constants.IdentityConstants;

namespace IdentityServer.Configurations;

public static class IdentityServerConfiguration
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConnectionString = configuration.GetConnectionString(ConnectionStringsConstants.DatabaseConnection)!;
        var identityOptions = configuration.GetSection(IdentityConstants.SectionName).Get<IdentityOptions>()!;
        var assembly = typeof(Program).Assembly.GetName().Name;

        services.AddDbContext<GamingStoreContext>(options => options.UseSqlServer(dbConnectionString));

        services
            .AddIdentity<User, IdentityRole<int>>(options => options = identityOptions)
            .AddEntityFrameworkStores<GamingStoreContext>()
            .AddDefaultTokenProviders();

        services
            .AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(dbConnectionString, options => options.MigrationsAssembly(assembly));
            })
            .AddInMemoryIdentityResources(GetIdentityResources())
            .AddInMemoryApiResources(GetApiResources())
            .AddInMemoryApiScopes(GetApiScopes())
            .AddInMemoryClients(GetClients())
            .AddAspNetIdentity<User>();

        services.AddScoped<IProfileService, ProfileService>();
    }

    public static IEnumerable<IdentityResource> GetIdentityResources() =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new(name: "profile", userClaims: new [] { "name" }, displayName: "User's profile data")
        };

    public static IEnumerable<ApiResource> GetApiResources() =>
        new ApiResource[]
        {
            new("gamingStoreApi", "Gaming Store")
            {
                Scopes = { "apiAccess" }
            }
        };

    public static IEnumerable<ApiScope> GetApiScopes() =>
        new ApiScope[]
        {
            new("apiAccess", "Gaming Store Access")
        };

    public static IEnumerable<Client> GetClients() =>
        new Client[]
        {
            new()
            {
                RequireConsent = false,
                ClientId = "postman_client",
                ClientName = "Postman Client",
                AllowedScopes = { "apiAccess", "openid", "profile" },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                ClientSecrets = { new Secret("test_client_secret".ToSha256()) },
                AccessTokenLifetime = 600
            }
        };
}
