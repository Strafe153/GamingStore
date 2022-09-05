using System.Drawing;

namespace Core.Interfaces.Repositories
{
    public interface IPictureRepository
    {
        Task<string> UploadAsync(Image image, string folderName, string identifier, string imageFormat);
        Task DeleteAsync(string imageLink);
    }
}
