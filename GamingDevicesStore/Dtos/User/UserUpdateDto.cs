using System.ComponentModel.DataAnnotations;
using GamingDevicesStore.Data;
using GamingDevicesStore.ValidationAttributes;

namespace GamingDevicesStore.Dtos.User
{
    public record UserUpdateDto
    {
        [StringLength(20, MinimumLength = 1)]
        [UniqueUsername]
        public string Username { get; init; } = string.Empty;

        public UserRole Role { get; init; } = UserRole.User;
    }
}
