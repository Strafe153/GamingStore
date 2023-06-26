using Newtonsoft.Json;

namespace Web.Configurations;

public static class ControllersConfiguration
{
    public static void ConfigureControllers(this IServiceCollection services)
    {
        services
            .AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            })
            .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly)
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
    }
}
