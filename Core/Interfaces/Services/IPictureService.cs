using System.Drawing;

namespace Core.Interfaces.Services
{
    public interface IPictureService
    {
        Task<string> UploadAsync(Image image, string folderName, string identifier, string imageFormat);
        Task DeleteAsync(string imageLink);
    }
}
