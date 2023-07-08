using Web.Configurations;
using WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLoggers();

builder.Services.ConfigureHealthChecks(builder.Configuration);
builder.Services.ConfigureRateLimiting(builder.Configuration);

builder.Services.AddRepositories();
builder.Services.AddCustomServices();

builder.Services.ConfigureApiVersioning();
builder.Services.ConfigureControllers();
builder.Services.ConfigureMediatR();
builder.Services.ConfigureFluentValidation();

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureRedis(builder.Configuration);
builder.Services.ConfigureAzure(builder.Configuration);

builder.Services.ConfigureIdentity();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();

builder.Services.ConfigureAutoMapper();

builder.Services.ConfigureSwagger();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ConfigureSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHealthChecks();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.UseCustomMiddleware();

app.MapControllers();

app.UseDatabaseMigrations();

app.Run();
