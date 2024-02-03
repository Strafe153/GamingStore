using Domain.Shared.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Web.Configurations;

public static class AuthenticationAndAuthorizationConfiguration
{
    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = configuration.GetConnectionString(ConnectionStringsConstants.IdentityServerConnection);
                options.Audience = "gamingStoreApi";
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                };
            });

    public static void ConfigureAuthorization(this IServiceCollection services) =>
        services.AddAuthorization(options =>
            options.AddPolicy(AuthorizationConstants.AdminOnlyPolicy, policy => policy.RequireRole("Admin")));
}
