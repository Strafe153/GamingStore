using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using System.Threading.RateLimiting;
using Web.Configurations.ConfigurationModels;

namespace Web.Configurations;

public static class RateLimitingConfiguration
{
    public static void ConfigureRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        var rateLimitOptions = new RateLimitOptions();
        configuration.GetSection(RateLimitOptions.RateLimitOptionsSectionName).Bind(rateLimitOptions);

        services.AddRateLimiter(options =>
        {
            options.AddTokenBucketLimiter("tokenBucket", tokenOptions =>
            {
                tokenOptions.TokenLimit = rateLimitOptions.TokenLimit;
                tokenOptions.TokensPerPeriod = rateLimitOptions.TokensPerPeriod;
                tokenOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(rateLimitOptions.ReplenishmentPeriod);
                tokenOptions.QueueLimit = rateLimitOptions.QueueLimit;
                tokenOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                tokenOptions.AutoReplenishment = rateLimitOptions.AutoReplenishment;
            });

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = (context, _) =>
            {
                context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.");
                return new ValueTask();
            };
        });
    }
}
