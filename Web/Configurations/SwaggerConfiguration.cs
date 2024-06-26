﻿using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;
using Presentation.Controllers.V1;
using System.Reflection;

namespace Web.Configurations;

public static class SwaggerConfiguration
{
	public static void ConfigureSwagger(this IServiceCollection services)
	{
		services.AddEndpointsApiExplorer();

		services.AddSwaggerGen(options =>
		{
			const string BearerScheme = "Bearer";

			var apiVersionDescriptionProvider = services
				.BuildServiceProvider()
				.GetRequiredService<IApiVersionDescriptionProvider>();

			foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
			{
				options.SwaggerDoc(description.GroupName, new OpenApiInfo
				{
					Title = $"{typeof(Program).Assembly.GetName().Name} {description.ApiVersion}",
					Version = description.ApiVersion.ToString()
				});
			}

			options.AddSecurityDefinition(BearerScheme, new OpenApiSecurityScheme
			{
				Name = "Authorization",
				Type = SecuritySchemeType.ApiKey,
				Scheme = BearerScheme,
				BearerFormat = "JWT",
				In = ParameterLocation.Header,
				Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer k35dl2slkbijbl29\"",
			});

			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = BearerScheme
						}
					},
					Array.Empty<string>()
				}
			});

			var presentationPath = AppContext.BaseDirectory.Replace(nameof(Web), nameof(Presentation));
			var xmlFileName = $"{typeof(UsersController).Assembly.GetName().Name}.xml";

			options.IncludeXmlComments(Path.Combine(presentationPath, xmlFileName));
		});
	}

	public static void ConfigureSwaggerUI(this WebApplication application)
	{
		application.UseSwagger();

		application.UseSwaggerUI(options =>
		{
			var provider = application.Services.GetRequiredService<IApiVersionDescriptionProvider>();

			foreach (var description in provider.ApiVersionDescriptions)
			{
				options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
			}
		});
	}
}
