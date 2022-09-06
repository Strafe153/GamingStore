using System.Drawing;

namespace Core.Interfaces.Services
{
    public interface IPictureService
    {
        Task<string> UploadAsync(string? picturePath, string blobFolder, string identifier);
        Task DeleteAsync(string imageLink);
    }
}
