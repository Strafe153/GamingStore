using Application.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Web.Configurations;

public static class RepositoriesConfiguration
{
	public static void AddRepositories(this IServiceCollection services) =>
		services
			.AddScoped<IUnitOfWork, UnitOfWork>()
			.AddScoped<IUserRepository, UserRepository>()
			.AddScoped<IRepository<Company>, CompanyRepository>()
			.AddScoped<IRepository<Device>, DeviceRepository>();
}
