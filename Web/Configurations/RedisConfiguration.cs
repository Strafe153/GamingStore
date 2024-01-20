using Domain.Shared.Constants;

namespace Web.Configurations;

public static class RedisConfiguration
{
    public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString(ConnectionStringsConstants.RedisConnection);
        });
    }
}
