using IdentityServer.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLoggers();

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.ConfigureCors(builder.Configuration);
app.UseIdentityServer();
app.ApplyDatabaseMigrations(builder.Configuration);

app.Run();
