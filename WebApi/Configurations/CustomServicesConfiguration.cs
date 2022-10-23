using Application.Services;
using Core.Entities;
using Core.Interfaces.Services;

namespace WebApi.Configurations;

public static class CustomServicesConfiguration
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IService<Company>, CompanyService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IPictureService, PictureService>();
        services.AddSingleton<ICacheService, CacheService>();
    }
}
