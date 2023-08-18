using IdentityServer.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLoggers(builder.Configuration);

builder.Services.AddServices(builder.Configuration);
builder.Services.ConfigureCors(builder.Configuration);

var app = builder.Build();

app.UseCors();
app.UseIdentityServer();
app.ApplyDatabaseMigrations(builder.Configuration);

app.Run();
