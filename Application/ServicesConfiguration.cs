using Application.Services;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServicesConfiguration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IService<Company>, CompanyService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IPictureService, PictureService>();
        services.AddScoped<ICacheService, CacheService>();
    }
}
