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

        public async Task<string> UploadAsync(Image image, string folderName, string identifier, string imageFormat)
        {
            string fileName;

            try
            {
                fileName = await _pictureRepository.UploadAsync(image, folderName, identifier, imageFormat);
            }
            catch (Exception)
            {
                _logger.LogWarning("The image with the provided link could no be uploaded");
                throw new NullReferenceException("The image with the provided link could no be uploaded");
            }

            return fileName;
        }
    }
}
