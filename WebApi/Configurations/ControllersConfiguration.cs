using Newtonsoft.Json;

namespace WebApi.Configurations;

public static class ControllersConfiguration
{
    public static void ConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        })
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });
    }
}
