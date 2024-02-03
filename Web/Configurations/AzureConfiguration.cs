using Microsoft.Extensions.Azure;
using Application.Common.Extensions;
using Domain.Shared.Constants;

namespace Web.Configurations;

public static class AzureConfiguration
{
    public static void ConfigureAzure(this IServiceCollection services, IConfiguration configuration) =>
        services.AddAzureClients(clientBuilder =>
            clientBuilder.AddBlobServiceClient(
                configuration[$"{ConnectionStringsConstants.BlobStorageConnection}:blob"]!,
                preferMsi: true));
}
