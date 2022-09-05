using Azure.Storage.Blobs;
using Core.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System.Drawing;

namespace DataAccess.Repositories
{
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

        public async Task<string> UploadAsync(Image image, string folderName, string identifier, string imageFormat)
        {
            var fileName = $"{folderName}/{identifier}-{Guid.NewGuid()}.{imageFormat}";
            var blobItems = _blobContainerClient.GetBlobs(prefix: $"{folderName}/{identifier}");

            if (blobItems.Any())
            {
                await DeleteAsync(blobItems.First().Name);
            }

            using (MemoryStream ms = new())
            {
                image.Save(ms, image.RawFormat);
                ms.Position = 0;

                await _blobContainerClient.UploadBlobAsync(fileName, ms);
            }

            fileName = $"{_configuration["Azure:ContainerLink"]}/{_containerName}/{fileName}";

            return fileName;
        }

        public async Task DeleteAsync(string imageLink)
        {
            if (imageLink.Contains(_containerName))
            {
                imageLink = imageLink.Split(_containerName)[1];
            }

            var blobClient = _blobContainerClient.GetBlobClient(imageLink);

            await blobClient.DeleteAsync();
        }
    }
}
