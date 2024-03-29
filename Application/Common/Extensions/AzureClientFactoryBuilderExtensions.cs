﻿using Azure.Core.Extensions;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;

namespace Application.Common.Extensions;

public static class AzureClientFactoryBuilderExtensions
{
    public static IAzureClientBuilder<BlobServiceClient, BlobClientOptions> AddBlobServiceClient(
        this AzureClientFactoryBuilder builder,
        string serviceUriOrConnectionString,
        bool preferMsi)
    {
        if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri? serviceUri))
        {
            return builder.AddBlobServiceClient(serviceUri);
        }

        return builder.AddBlobServiceClient(serviceUriOrConnectionString);
    }
}
