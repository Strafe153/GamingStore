namespace Core.Dtos.UserDtos
{
    public record UserWithTokenReadDto : UserReadDto
    {
        public string? Token { get; init; }
    }
}
