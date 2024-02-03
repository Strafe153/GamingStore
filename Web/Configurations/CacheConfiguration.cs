using Domain.Shared;

namespace Web.Configurations;

public static class CacheConfiguration
{
    public static void ConfigureCache(this IServiceCollection services, IConfiguration configuration) =>
        services.Configure<CacheOptions>(configuration.GetSection(CacheOptions.SectionName));
}
