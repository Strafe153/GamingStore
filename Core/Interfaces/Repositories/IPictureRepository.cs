using SixLabors.ImageSharp;

namespace Core.Interfaces.Repositories;

public interface IPictureRepository
{
    Task<string> UploadAsync(Image image, string blobFolder, string identifier, string extension);
    Task DeleteAsync(string imageLink);
}
