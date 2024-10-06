using Domain.Entities;
using Elasticsearch.Net;
using Nest;
using System.Reflection;

namespace Web.Configurations;

public static class ElasticSearchConfiguration
{
    public static void ConfigureElasticSearch(this IServiceCollection services, IConfiguration configuration)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
        var pool = new SingleNodeConnectionPool(new Uri(configuration.GetConnectionString("ElasticSearchConnection")!));

        var settings = new ConnectionSettings(pool)
            .DefaultIndex($"{Assembly.GetExecutingAssembly().GetName()?.Name?.ToLower().Replace('.', '-')}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM}")
            .DefaultMappingFor<Log>(options =>
                options
                    .PropertyName(l => l.Message, "message")
                    .PropertyName(l => l.Timestamp, "@timestamp")
                    .PropertyName(l => l.Level, "level")
                    .PropertyName(l => l.Environment, "fields.Environment"))
            .DefaultFieldNameInferrer(l => l);

        var client = new ElasticClient(settings);

        services.AddSingleton(client);
    }
}
