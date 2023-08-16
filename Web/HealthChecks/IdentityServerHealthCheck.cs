using Microsoft.Extensions.Diagnostics.HealthChecks;
using Web.HttpClients;

namespace Web.HealthChecks;

public class IdentityServerHealthCheck : IHealthCheck
{
    private readonly IdentityServerClient _client;

    public IdentityServerHealthCheck(IdentityServerClient client)
    {
        _client = client;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _client.CheckIdentityServerHealth();
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
