using Application.AutoMapperProfiles;
using Web.Configurations;
using Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLoggers();

builder.Services.ConfigureHealthChecks(builder.Configuration);
builder.Services.ConfigureRateLimiting(builder.Configuration);
builder.Services.ConfigureHttpClients(builder.Configuration);

builder.Services.AddRepositories();
builder.Services.AddCustomServices();

builder.Services.ConfigureApiVersioning();
builder.Services.ConfigureControllers();
builder.Services.ConfigureMediatR();
builder.Services.ConfigureFluentValidation();

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureCache(builder.Configuration);
builder.Services.ConfigureRedis(builder.Configuration);
builder.Services.ConfigureAzure(builder.Configuration);

builder.Services.ConfigureIdentity();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();

builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
builder.Services.ConfigureNSwag();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();

app.UseHealthChecks();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.UseDatabaseMigrations();

app.Run();