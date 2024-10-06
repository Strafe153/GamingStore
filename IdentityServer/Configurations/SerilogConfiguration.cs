using Domain.Shared;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace IdentityServer.Configurations;

public static class SerilogConfiguration
{
    public static void ConfigureSerilog(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;

        var logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console(outputTemplate: LoggerConstants.OUTPUT_TEMPLATE)
            .WriteTo.File(
                path: "./logs/IdentityServer.log",
                rollingInterval: RollingInterval.Day,
                outputTemplate: LoggerConstants.OUTPUT_TEMPLATE)
            .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
            .Enrich.WithProperty("Environment", environment)
            .CreateLogger();

        builder.Logging
            .ClearProviders()
            .AddSerilog(logger);
    }

    private static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string environment) =>
        new ElasticsearchSinkOptions(new Uri(configuration.GetConnectionString("ElasticSearchConnection")!))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"{Assembly.GetExecutingAssembly().GetName()?.Name?.ToLower().Replace('.', '-')}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
            NumberOfReplicas = 1,
            NumberOfShards = 2
        };
}
