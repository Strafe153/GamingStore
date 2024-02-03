using Application.HttpClients;
using Domain.Shared.Constants;
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
            .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)));
}
