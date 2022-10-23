using IdentityServer.Configurations;
using IdentityServer.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLoggers();

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});

app.UseIdentityServer();

app.ApplyDatabaseMigrations(builder.Configuration);

app.Run();
