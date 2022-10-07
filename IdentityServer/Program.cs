using IdentityServer.ServiceExtensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Create a serilog instance
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders()
    .AddSerilog(logger);

builder.Services.AddServices(config);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(options =>
    {
        options.WithOrigins("https://localhost:5001", "http://localhost:5136");
    });
});

var app = builder.Build();

app.UseIdentityServer();

app.Run();
