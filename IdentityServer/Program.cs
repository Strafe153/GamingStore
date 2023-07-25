using IdentityServer.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLoggers(builder.Configuration);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.ConfigureCors(builder.Configuration);
app.UseIdentityServer();
app.ApplyDatabaseMigrations(builder.Configuration);

app.Run();
