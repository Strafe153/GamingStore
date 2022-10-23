using DataAccess.Extensions;
using Microsoft.Extensions.Azure;

namespace WebApi.Configurations;

public static class AzureConfiguration
{
    public static void ConfigureAzure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAzureClients(clientBuilder =>
        {
            clientBuilder.AddBlobServiceClient(configuration["BlobStorageConnection"], preferMsi: true);
        });
    }
}
