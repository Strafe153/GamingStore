using Presentation.AutoMapperProfiles;
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
builder.Services.ConfigureRedis(builder.Configuration);
builder.Services.ConfigureAzure(builder.Configuration);

builder.Services.ConfigureIdentity();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();

builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
builder.Services.ConfigureSwagger();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

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

app.MapControllers();

app.UseDatabaseMigrations();

app.Run();