namespace Core.Dtos.UserDtos
{
    public record UserLoginDto
    {
        public string? Username { get; init; }
        public string? Password { get; init; }
    }
}
