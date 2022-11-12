using FluentValidation;

namespace Web.Configurations;

public static class FluentValidationConfiguration
{
    public static void ConfigureFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly, includeInternalTypes: true);
    }
}
