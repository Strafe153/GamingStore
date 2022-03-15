using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using GamingDevicesStore.Data;
using GamingDevicesStore.Models;
using GamingDevicesStore.Repositories.Interfaces;
using GamingDevicesStore.Repositories.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("AppSettings:Token").Value))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireRole("Admin");
    });
});

builder.Services.AddScoped<IUserControllable, UsersRepository>();
builder.Services.AddScoped<IControllable<Company>, CompaniesRepository>();
builder.Services.AddScoped<IControllable<Device>, DevicesRepository>();

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
