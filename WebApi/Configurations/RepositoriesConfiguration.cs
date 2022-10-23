using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Repositories;

namespace WebApi.Configurations;

public static class RepositoriesConfiguration
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRepository<Company>, CompanyRepository>();
        services.AddScoped<IRepository<Device>, DeviceRepository>();
    }
}
