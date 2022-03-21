using System.ComponentModel.DataAnnotations;

namespace GamingStore.Dtos.User
{
    public record UserLoginDto
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Username { get; init; } = string.Empty;

        [Required]
        [StringLength(16, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; init; } = string.Empty;
    }
}
