using Core.Enums;

namespace Core.ViewModels.UserViewModels
{
    public record UserChangeRoleViewModel
    {
        public UserRole Role { get; init; } = UserRole.User;
    }
}
