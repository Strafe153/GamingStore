using Application.HttpClients;
using Domain.Shared.Constants;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace Web.Configurations;

public static class HttpClientsConfiguration
{
    public static void ConfigureHttpClients(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddHttpClient<IdentityServerClient>(options =>
            {
                options.BaseAddress = new Uri(
                    configuration.GetConnectionString(ConnectionStringsConstants.IdentityServerConnection)!);
            })
            .AddResilienceHandler("identityServerResiliencePipeline", builder =>
            {
                builder.AddRetry(
                    new HttpRetryStrategyOptions
                    {
                        MaxRetryAttempts = 3,
                        BackoffType = DelayBackoffType.Exponential,
                        Delay = TimeSpan.FromSeconds(.5),
                        UseJitter = true
                    });

                builder.AddTimeout(TimeSpan.FromSeconds(3));
            });
}
