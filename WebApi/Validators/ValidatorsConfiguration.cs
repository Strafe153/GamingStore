using Core.ViewModels.CompanyViewModels;
using Core.ViewModels.DeviceViewModels;
using Core.ViewModels.UserViewModels;
using FluentValidation;
using WebApi.Validators.CompanyValidators;
using WebApi.Validators.DeviceValidators;
using WebApi.Validators.UserValidators;

namespace WebApi.Validators
{
    public static class ValidatorsConfiguration
    {
        public static void AddApplicationValidators(this IServiceCollection services)
        {
            // User validators
            services.AddScoped<IValidator<UserBaseViewModel>, UserBaseValidator<UserBaseViewModel>>();
            services.AddScoped<IValidator<UserChangeRoleViewModel>, UserChangeRoleValidator>();
            services.AddScoped<IValidator<UserChangePasswordViewModel>, UserChangePasswordValidator>();

            // Company validators
            services.AddScoped<IValidator<CompanyBaseViewModel>, CompanyBaseValidator>();

            // Device validators
            services.AddScoped<IValidator<DeviceBaseViewModel>, DeviceBaseValidator>();
        }
    }
}
