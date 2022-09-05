using Core.Enums;

namespace Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public UserRole Role { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
