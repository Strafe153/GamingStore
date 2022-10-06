namespace Core.Dtos.UserDtos;

public record UserChangePasswordDto
{
    public string? CurrentPassword { get; init; }
    public string? NewPassword { get; init; }
}
