using Application.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Web.Configurations;

public static class RepositoriesConfiguration
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRepository<Company>, CompanyRepository>();
        services.AddScoped<IRepository<Device>, DeviceRepository>();
    }
}
