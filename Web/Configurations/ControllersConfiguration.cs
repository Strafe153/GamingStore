using System.Text.Json.Serialization;

namespace Web.Configurations;

public static class ControllersConfiguration
{
    public static void ConfigureControllers(this IServiceCollection services) =>
        services
            .AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
}
