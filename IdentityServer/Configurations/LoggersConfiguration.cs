using Serilog;

namespace IdentityServer.Configurations;

public static class LoggersConfiguration
{
    public static void ConfigureLoggers(this WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Logging.ClearProviders()
            .AddSerilog(logger);
    }
}
