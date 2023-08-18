using IdentityServer.Configurations.ConfigurationModels;

namespace IdentityServer.Configurations;

public static class CorsConfiguration
{
    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        var corsOptions = new CorsOptions();
        configuration.GetSection("Cors").Bind(corsOptions);

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(corsOptions.Origins);
                policy.WithMethods(corsOptions.Methods);
                policy.WithHeaders(corsOptions.Headers);
            });
        });
    }
}
