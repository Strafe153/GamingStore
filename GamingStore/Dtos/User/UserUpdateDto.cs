using System.ComponentModel.DataAnnotations;
using GamingStore.Data;

namespace GamingStore.Dtos.User
{
    public record UserUpdateDto
    {
        [StringLength(20, MinimumLength = 1)]
        public string Username { get; init; } = string.Empty;

        public UserRole Role { get; init; } = UserRole.User;
        public string ProfilePicture { get; init; } = string.Empty;
    }
}
