using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace Web.HealthChecks;

public class IdentityServerHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public IdentityServerHealthCheck(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            await httpClient.PostAsync(
                $"{_configuration.GetConnectionString("IdentityServerConnection")}/connect/token",
                JsonContent.Create(JsonConvert.SerializeObject("{}")));

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
