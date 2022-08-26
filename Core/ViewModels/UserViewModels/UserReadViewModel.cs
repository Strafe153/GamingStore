using Core.Enums;

namespace Core.ViewModels.UserViewModels
{
    public record UserReadViewModel : UserBaseViewModel
    {
        public int Id { get; init; }
        public UserRole Role { get; init; }
    }
}
