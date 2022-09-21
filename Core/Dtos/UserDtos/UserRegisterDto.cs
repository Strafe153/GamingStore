using Microsoft.AspNetCore.Http;

namespace Core.Dtos.UserDtos;

public record UserRegisterDto : UserBaseDto
{
    public string? Password { get; init; }
    public IFormFile? ProfilePicture { get; init; }
}
