using Domain.Shared.Constants;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Web.HealthChecks;

namespace Web.Configurations;

public static class HealthChecksConfiguration
{
    public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString(ConnectionStringsConstants.DatabaseConnection)!)
            .AddRedis(configuration.GetConnectionString(ConnectionStringsConstants.RedisConnection)!)
            .AddAzureBlobStorage(configuration.GetConnectionString(ConnectionStringsConstants.BlobStorageConnection)!)
            .AddElasticsearch(configuration.GetConnectionString(ConnectionStringsConstants.ElasticSearchConnection)!)
            .AddCheck<IdentityServerHealthCheck>("IdentityServer");
    }

    public static void UseHealthChecks(this WebApplication application)
    {
        application.MapHealthChecks("/_health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }
}
