using GamingDevicesStore.Data;

namespace GamingDevicesStore.Dtos.User
{
    public record UserWithTokenReadDto
    {
        public Guid Id { get; init; }
        public string Username { get; init; } = string.Empty;
        public UserRole Role { get; init; }
        public string Token { get; init; } = string.Empty;
    }
}
