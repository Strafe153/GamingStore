using Azure;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Drawing;

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

        public async Task<string> UploadAsync(string? picturePath, string blobFolder, string identifier)
        {
            string fileName;

            try
            {
                fileName = await GetPictureLinkAsync(picturePath, blobFolder, identifier);
            }
            catch (Exception) // RequestFailedException
            {
                _logger.LogWarning("The image with the provided link could no be uploaded");
                throw new NullReferenceException("The image with the provided link could no be uploaded");
            }

            return fileName;
        }

        private async Task<string> GetPictureLinkAsync(string? picturePath, string blobFolder, string identifier)
        {
            byte[]? bytes;

            if (picturePath is not null)
            {
                bytes = await File.ReadAllBytesAsync(picturePath);
            }
            else
            {
                string defaultProfilePicPath = "../WebApi/Assets/Images/default_profile_pic.jpg";
                bytes = await File.ReadAllBytesAsync(defaultProfilePicPath);
            }

            using (MemoryStream ms = new(bytes))
            {
                var image = Image.FromStream(ms);
                string profilePictureLink = await _pictureRepository.UploadAsync(image, blobFolder, identifier);

                return profilePictureLink;
            }
        }
    }
}
