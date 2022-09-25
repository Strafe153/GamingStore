using Azure.Storage.Blobs;
using Core.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;

namespace DataAccess.Repositories;

public class PictureRepository : IPictureRepository
{
    private readonly IConfiguration _configuration;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;
    private readonly BlobContainerClient _blobContainerClient;

    public PictureRepository(
        IConfiguration configuration,
        BlobServiceClient blobServiceClient)
    {
        _configuration = configuration;
        _blobServiceClient = blobServiceClient;
        _containerName = _configuration["Azure:ContainerName"];
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
    }

    public async Task<string> UploadAsync(Image image, string blobFolder, string identifier, string extension)
    {
        var fileName = $"{blobFolder}/{identifier}-{Guid.NewGuid()}.{extension}";

        using (MemoryStream ms = new())
        {
            image.SaveAsPng(ms);
            ms.Position = 0;

            await _blobContainerClient.UploadBlobAsync(fileName, ms);
        }

        fileName = $"{_configuration["Azure:ContainerLink"]}/{_containerName}/{fileName}";

        return fileName;
    }

    public async Task DeleteAsync(string imagePath)
    {
        if (imagePath.Contains(_containerName))
        {
            imagePath = imagePath.Split(_containerName)[1];
        }

        var blobClient = _blobContainerClient.GetBlobClient(imagePath);

        await blobClient.DeleteAsync();
    }
}
