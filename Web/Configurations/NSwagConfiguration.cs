using Asp.Versioning.ApiExplorer;
using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors.Security;

namespace Web.Configurations;

public static class NSwagConfiguration
{
    public static void ConfigureNSwag(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        var descriptionProvider = services
            .BuildServiceProvider()
            .GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in descriptionProvider.ApiVersionDescriptions)
        {
            services.RegisterApiDocument(description);
        }
    }

    private static void RegisterApiDocument(this IServiceCollection services, ApiVersionDescription description)
    {
        services.AddOpenApiDocument(options =>
        {
            options.DocumentName = description.GroupName;
            options.Title = $"{typeof(Program).Assembly.GetName().Name} {description.ApiVersion}";
            options.Version = description.ApiVersion.ToString();
            options.ApiGroupNames = new[] { description.GroupName };

            options.RegisterBearerScheme();
        });
    }

    private static void RegisterBearerScheme(this AspNetCoreOpenApiDocumentGeneratorSettings options)
    {
        const string BearerScheme = "Bearer";

        OpenApiSecurityScheme bearerScheme = new()
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer kl4wof7t3nf\"",
            BearerFormat = "JWT"
        };

        OperationSecurityScopeProcessor bearerProcessor = new(BearerScheme);

        options.AddSecurity(BearerScheme, bearerScheme);
        options.OperationProcessors.Add(bearerProcessor);
    }
}