using Azure;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

namespace Application.Services
{
    public class PictureService : IPictureService
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly ILogger<PictureService> _logger;
        private readonly string[] _imageExtensions = new[]
        {
            "jpg", "jpeg", "png", "gif", "webp", "tif", "tiff"
        };

        public PictureService(
            IPictureRepository pictureRepository,
            ILogger<PictureService> logger)
        {
            _pictureRepository = pictureRepository;
            _logger = logger;
        }

        public async Task DeleteAsync(string imageLink)
        {
            try
            {
                await _pictureRepository.DeleteAsync(imageLink);
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
                _logger.LogWarning($"The file with the extension '.{extension}' could not be uploaded");
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

        private async Task<string> GetPictureLinkAsync(
            byte[]? formFileAsBytes, string blobFolder, string identifier, string extension)
        {
            if (formFileAsBytes is null)
            {
                string defaultProfilePicPath = "../Application/Assets/Images/default_profile_pic.jpg";
                formFileAsBytes = await File.ReadAllBytesAsync(defaultProfilePicPath);
            }

            using (MemoryStream ms = new(formFileAsBytes))
            {
                var image = Image.Load(ms);
                string pictureLink = await _pictureRepository.UploadAsync(image, blobFolder, identifier, extension);

                return pictureLink;
            }
        }
    }
}
