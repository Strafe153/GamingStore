using Azure;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace Application.Services
{
    public class PictureService : IPictureService
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly ILogger<PictureService> _logger;

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
                byte[]? pictureAsBase64 = ReadFromIFormFile(picture);
                fileName = await GetPictureLinkAsync(pictureAsBase64, blobFolder, identifier);
            }
            catch (RequestFailedException)
            {
                _logger.LogWarning("The image with the provided path could no be uploaded");
                throw new NullReferenceException("The image with the provided path could no be uploaded");
            }

            return fileName;
        }

        private byte[]? ReadFromIFormFile(IFormFile? formFile)
        {
            if (formFile is not null)
            {
                using (var ms = new MemoryStream())
                {
                    formFile.CopyTo(ms);
                    byte[] formFileAsBytes = ms.ToArray();

                    return formFileAsBytes;
                }
            }

            return null;
        }

        private async Task<string> GetPictureLinkAsync(byte[]? formFileAsBytes, string blobFolder, string identifier)
        {
            if (formFileAsBytes is null)
            {
                string defaultProfilePicPath = "../Application/Assets/Images/default_profile_pic.jpg";
                formFileAsBytes = await File.ReadAllBytesAsync(defaultProfilePicPath);
            }

            using (MemoryStream ms = new(formFileAsBytes))
            {
                var image = Image.Load(ms);
                string profilePictureLink = await _pictureRepository.UploadAsync(image, blobFolder, identifier);

                return profilePictureLink;
            }
        }
    }
}
