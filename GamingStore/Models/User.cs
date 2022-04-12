using GamingStore.Data;

namespace GamingStore.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;
        public byte[]? ProfilePicture { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
    }
}