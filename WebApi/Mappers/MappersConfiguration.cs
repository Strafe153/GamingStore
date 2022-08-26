using Core.Entities;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.CompanyViewModels;
using Core.ViewModels.DeviceViewModels;
using Core.ViewModels.UserViewModels;
using WebApi.Mappers.CompanyMappers;
using WebApi.Mappers.DeviceMappers;
using WebApi.Mappers.Interfaces;
using WebApi.Mappers.UserMappers;

namespace WebApi.Mappers
{
    public static class MappersConfiguration
    {
        public static void AddApplicationMappers(this IServiceCollection services)
        {
            // User mappers
            services.AddScoped<IMapper<User, UserReadViewModel>, UserReadMapper>();
            services.AddScoped<IMapper<PaginatedList<User>, PageViewModel<UserReadViewModel>>, UserPaginatedMapper>();
            services.AddScoped<IMapper<User, UserWithTokenReadViewModel>, UserReadWithTokenMapper>();

            // Company mappers
            services.AddScoped<IMapper<Company, CompanyReadViewModel>, CompanyReadMapper>();
            services.AddScoped<IMapper<PaginatedList<Company>, PageViewModel<CompanyReadViewModel>>, CompanyPaginatedMapper>();
            services.AddScoped<IMapper<CompanyBaseViewModel, Company>, CompanyCreateMapper>();
            services.AddScoped<IUpdateMapper<CompanyBaseViewModel, Company>, CompanyUpdateMapper>();

            // Device mappers
            services.AddScoped<IMapper<Device, DeviceReadViewModel>, DeviceReadMapper>();
            services.AddScoped<IMapper<PaginatedList<Device>, PageViewModel<DeviceReadViewModel>>, DevicePaginatedMapper>();
            services.AddScoped<IMapper<DeviceBaseViewModel, Device>, DeviceCreateUpdateMapper>();
            services.AddScoped<IUpdateMapper<DeviceBaseViewModel, Device>, DeviceUpdateMapper>();
        }
    }
}
