using Application.Abstractions.Services;
using Application.Services;

namespace Web.Configurations;

public static class CustomServicesConfiguration
{
	public static void AddCustomServices(this IServiceCollection services) =>
		services
			.AddScoped<IUserService, UserService>()
			.AddScoped<IPictureService, PictureService>()
			.AddSingleton<ICacheService, CacheService>();
}
