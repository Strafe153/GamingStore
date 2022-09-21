namespace Core.Dtos.UserDtos;

public record UserBaseDto
{
    public string? Username { get; init; }
    public string? Email { get; init; }
}
