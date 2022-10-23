using Azure;
using Azure.Storage.Blobs;
using Core.Exceptions;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace Application.Services;

public class PictureService : IPictureService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<PictureService> _logger;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;
    private readonly BlobContainerClient _blobContainerClient;
    private readonly string[] _imageExtensions = new[]
    {
        "jpg", "jpeg", "png", "gif", "webp", "tif", "tiff"
    };

    public PictureService(
        IConfiguration configuration,
        ILogger<PictureService> logger,
        BlobServiceClient blobServiceClient)
    {
        _configuration = configuration;
        _logger = logger;
        _blobServiceClient = blobServiceClient;
        _containerName = _configuration["Azure:ContainerName"];
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
    }

    public async Task DeleteAsync(string imagePath)
    {
        try
        {
            if (imagePath.Contains(_containerName))
            {
                imagePath = imagePath.Split(_containerName)[1];
            }

            var blobClient = _blobContainerClient.GetBlobClient(imagePath);

            await blobClient.DeleteAsync();
        }
        catch (RequestFailedException)
        {
            _logger.LogWarning("The image with the provided link could no be deleted");
            throw new NullReferenceException("The image with the provided link could no be deleted");
        }
    }

    public async Task<string> UploadAsync(IFormFile? picture, string blobFolder, string identifier)
    {
        string fileName;

        try
        {
            (byte[]? pictureAsBase64, string extension) = ReadFromIFormFile(picture);
            fileName = await GetPictureLinkAsync(pictureAsBase64, blobFolder, identifier, extension);
        }
        catch (RequestFailedException)
        {
            _logger.LogWarning("The image with the provided path could no be uploaded");
            throw new NullReferenceException("The image with the provided path could no be uploaded");
        }

        return fileName;
    }

    private string VerifyFileExtension(IFormFile formFile)
    {
        string extension = formFile.ContentType.Split('/').Last();

        if (!_imageExtensions.Contains(extension))
        {
            _logger.LogWarning("The file with the extension '.{Extension}' could not be uploaded", extension);
            throw new IncorrectExtensionException($"The file with the extension '.{extension}' could not be uploaded");
        }

        return extension;
    }

    private (byte[]?, string) ReadFromIFormFile(IFormFile? formFile)
    {
        if (formFile is not null)
        {
            string extension = VerifyFileExtension(formFile);

            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                byte[] formFileAsBytes = ms.ToArray();

                return (formFileAsBytes, extension);
            }
        }

        return (null, "png");
    }

    private async Task<string> UploadToBlobStorageAsync(Image image, string blobFolder, string identifier, string extension)
    {
        string fileName = $"{blobFolder}/{identifier}-{Guid.NewGuid()}.{extension}";

        using (var ms = new MemoryStream())
        {
            image.SaveAsPng(ms);
            ms.Position = 0;

            await _blobContainerClient.UploadBlobAsync(fileName, ms);
        }

        fileName = $"{_configuration["Azure:ContainerLink"]}/{_containerName}/{fileName}";

        return fileName;
    }

    private async Task<string> GetPictureLinkAsync(
        byte[]? formFileAsBytes, string blobFolder, string identifier, string extension)
    {
        if (formFileAsBytes is null)
        {
            string defaultPicturePath = _configuration.GetSection("Application:DefaultPicturePath").Value;
            formFileAsBytes = await File.ReadAllBytesAsync(defaultPicturePath);
        }

        using (var ms = new MemoryStream(formFileAsBytes))
        {
            var image = Image.Load(ms);
            string pictureLink = await UploadToBlobStorageAsync(image, blobFolder, identifier, extension);

            return pictureLink;
        }
    }
}
