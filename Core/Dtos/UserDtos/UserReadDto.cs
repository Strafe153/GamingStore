using Core.Enums;

namespace Core.Dtos.UserDtos
{
    public record UserReadDto : UserBaseDto
    {
        public int Id { get; init; }
        public UserRole Role { get; init; }
        public string? ProfilePicture { get; init; }
    }
}
