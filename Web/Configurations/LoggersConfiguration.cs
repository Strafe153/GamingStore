using Domain.Shared.Constants;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace Web.Configurations;

public static class LoggersConfiguration
{
    public static void ConfigureLoggers(this WebApplicationBuilder builder)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;

        var logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console(outputTemplate: LoggerConstants.OutputTemplate)
            .WriteTo.File(
                path: "./logs/GamingStore.log",
                rollingInterval: RollingInterval.Day,
                outputTemplate: LoggerConstants.OutputTemplate)
            .WriteTo.Elasticsearch(ConfigureElasticSink(builder.Configuration, environment))
            .Enrich.WithProperty("Environment", environment)
            .CreateLogger();

        builder.Logging
            .ClearProviders()
            .AddSerilog(logger);
    }

    private static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string environment) =>
        new(new Uri(configuration.GetConnectionString(ConnectionStringsConstants.ElasticSearchConnection)!))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower().Replace('.', '-')}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
            NumberOfReplicas = 1,
            NumberOfShards = 2
        };
}
