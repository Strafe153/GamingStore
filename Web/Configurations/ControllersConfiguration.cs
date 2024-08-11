using System.Text.Json.Serialization;

namespace Web.Configurations;

public static class ControllersConfiguration
{
    public static void ConfigureControllers(this IServiceCollection services) =>
        services
            .AddControllers()
            .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly)
            .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
}
