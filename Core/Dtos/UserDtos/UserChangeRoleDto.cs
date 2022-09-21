using Core.Enums;

namespace Core.Dtos.UserDtos;

public record UserChangeRoleDto
{
    public UserRole Role { get; init; } = UserRole.User;
}
