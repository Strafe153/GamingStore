using GamingStore.Data;

namespace GamingStore.Dtos.User
{
    public record UserReadDto
    {
        public Guid Id { get; init; }
        public string Username { get; init; } = string.Empty;
        public UserRole Role { get; init; }
        public byte[]? ProfilePicture { get; init; }
    }
}
