using Core.Dtos.CompanyDtos;
using Core.Dtos.DeviceDtos;
using Core.Dtos.UserDtos;
using FluentValidation;
using FluentValidation.AspNetCore;
using WebApi.Validators.CompanyValidators;
using WebApi.Validators.DeviceValidators;
using WebApi.Validators.UserValidators;

namespace WebApi.Configurations;

public static class FluentValidationConfiguration
{
    public static void ConfigureFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();
    }
    
    public static void AddCustomValidators(this IServiceCollection services)
    {
        // User validators
        services.AddScoped<IValidator<UserBaseDto>, UserBaseValidator<UserBaseDto>>();
        services.AddScoped<IValidator<UserRegisterDto>, UserBaseValidator<UserRegisterDto>>();
        services.AddScoped<IValidator<UserLoginDto>, UserLoginValidator>();
        services.AddScoped<IValidator<UserUpdateDto>, UserUpdateValidator>();
        services.AddScoped<IValidator<UserChangePasswordDto>, UserChangePasswordValidator>();

        // Company validators
        services.AddScoped<IValidator<CompanyBaseDto>, CompanyBaseValidator>();

        // Device validators
        services.AddScoped<IValidator<DeviceBaseDto>, DeviceBaseValidator>();
    }
}
