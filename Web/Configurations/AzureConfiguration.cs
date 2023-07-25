using Microsoft.Extensions.Azure;
using Application.Common.Extensions;

namespace Web.Configurations;

public static class AzureConfiguration
{
    public static void ConfigureAzure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAzureClients(clientBuilder =>
        {
            clientBuilder.AddBlobServiceClient(configuration["BlobStorageConnection:blob"]!, preferMsi: true);
        });
    }
}
