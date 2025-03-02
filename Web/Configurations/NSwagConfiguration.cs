using Asp.Versioning.ApiExplorer;
using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors.Security;

namespace Web.Configurations;

public static class NSwagConfiguration
{
    private const string _bearer = "Bearer";
    private const string _authorization = "Authorization";
    private const string _jwtFormat = "JWT";

    const string _description = """
        Enter 'Bearer' [space] and then your token in the text input below.
        Example: "Bearer kl4wof7t3nf"
    """;

    private static readonly string? _assemblyName = typeof(Program).Assembly.GetName().Name;

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
        var title = $"{_assemblyName} {description.ApiVersion}";

        services.AddOpenApiDocument(options =>
        {
            options.DocumentName = description.GroupName;
            options.Title = title;
            options.Version = description.ApiVersion.ToString();
            options.ApiGroupNames = [description.GroupName];

            options.RegisterBearerScheme();
        });
    }

    private static void RegisterBearerScheme(this AspNetCoreOpenApiDocumentGeneratorSettings options)
    {
        OpenApiSecurityScheme bearerScheme = new()
        {
            Name = _authorization,
            BearerFormat = _jwtFormat,
            Description = _description,
            Type = OpenApiSecuritySchemeType.ApiKey,
            In = OpenApiSecurityApiKeyLocation.Header
        };

        OperationSecurityScopeProcessor bearerProcessor = new(_bearer);

        options.AddSecurity(_bearer, bearerScheme);
        options.OperationProcessors.Add(bearerProcessor);
    }
}