using Microsoft.AspNetCore.Http;

namespace Core.Dtos.UserDtos
{
    public record UserUpdateDto : UserBaseDto
    {
        public IFormFile? ProfilePicture { get; init; }
    }
}
