using Serilog;

namespace Web.Configurations;

public static class LoggersConfiguration
{
    public static void ConfigureLoggers(this WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        // Configure logging.
        builder.Logging.ClearProviders()
            .AddSerilog(logger);
    }
}
