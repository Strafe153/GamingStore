using Core.Dtos.CompanyDtos;
using Core.Dtos.DeviceDtos;
using Core.Dtos.UserDtos;
using FluentValidation;
using WebApi.Validators.CompanyValidators;
using WebApi.Validators.DeviceValidators;
using WebApi.Validators.UserValidators;

namespace WebApi.Validators;

public static class ValidatorsConfiguration
{
    public static void AddApplicationValidators(this IServiceCollection services)
    {
        // User validators
        services.AddScoped<IValidator<UserBaseDto>, UserBaseValidator<UserBaseDto>>();
        services.AddScoped<IValidator<UserRegisterDto>, UserBaseValidator<UserRegisterDto>>();
        services.AddScoped<IValidator<UserLoginDto>, UserLoginValidator>();
        services.AddScoped<IValidator<UserUpdateDto>, UserUpdateValidator>();
        services.AddScoped<IValidator<UserChangeRoleDto>, UserChangeRoleValidator>();
        services.AddScoped<IValidator<UserChangePasswordDto>, UserChangePasswordValidator>();

        // Company validators
        services.AddScoped<IValidator<CompanyBaseDto>, CompanyBaseValidator>();

        // Device validators
        services.AddScoped<IValidator<DeviceBaseDto>, DeviceBaseValidator>();
    }
}
