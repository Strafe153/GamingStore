using Microsoft.AspNetCore.Http;

namespace Core.Interfaces.Services;

public interface IPictureService
{
    Task<string> UploadAsync(IFormFile? picture, string blobFolder, string identifier);
    Task DeleteAsync(string imageLink);
}
