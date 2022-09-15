using Microsoft.AspNetCore.Http;

namespace Core.Dtos.UserDtos
{
    public record UserUpdateDto
    {
        public string? Username { get; init; }
        public IFormFile? ProfilePicture { get; init; }
    }
}
