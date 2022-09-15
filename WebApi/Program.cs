using Application;
using DataAccess;
using DataAccess.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using System.Text;
using WebApi.Middleware;
using WebApi.Validators;

var builder = WebApplication.CreateBuilder(args);

// Create a serilog instance.
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

// Configure logging.
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add custom validators, repositories, services and mappers.
builder.Services.AddApplicationValidators();
builder.Services.AddApplicationRepositories();
builder.Services.AddApplicationServices();

// Add, configure controllers.
builder.Services
    .AddControllers(options =>
    {
        options.SuppressAsyncSuffixInActionNames = false;
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

// Add database connection
builder.Services.AddDbContext<GamingStoreContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DatabaseConnection")));

// Add JWT-token authentication.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Add AuthorizationPolicy.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireRole("Admin");
    });
});

// Add fluent validation
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add BlobServiceClient.
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["BlobStorageConnection:blob"], preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["BlobStorageConnection:queue"], preferMsi: true);
});

// Add and configure Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement() 
    {
        {
            new OpenApiSecurityScheme() 
            {
                Reference = new OpenApiReference() 
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add custom middleware
app.AddApplicationMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
