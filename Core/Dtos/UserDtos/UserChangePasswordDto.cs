namespace Core.Dtos.UserDtos;

public record UserChangePasswordDto
{
    public string? Password { get; init; }
}
