using Microsoft.AspNetCore.Http;

namespace Core.Dtos.UserDtos;

public record UserUpdateDto
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? PhoneNumber { get; init; }
    public IFormFile? ProfilePicture { get; init; }
}
