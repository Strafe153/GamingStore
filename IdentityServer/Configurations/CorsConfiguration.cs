using IdentityServer.Configurations.ConfigurationModels;

namespace IdentityServer.Configurations;

public static class CorsConfiguration
{
    public static void ConfigureCors(this WebApplication app, IConfiguration configuration)
    {
        var corsOptions = new CorsOptions();
        configuration.GetSection("Cors").Bind(corsOptions);

        app.UseCors(options =>
        {
            options.AllowAnyOrigin();
            options.AllowAnyMethod();
            options.AllowAnyHeader();
            //options.WithOrigins(corsOptions.Origins);
            //options.WithMethods(corsOptions.Methods);
            //options.WithHeaders(corsOptions.Headers);
        });
    }
}
