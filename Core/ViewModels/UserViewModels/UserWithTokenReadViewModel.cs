namespace Core.ViewModels.UserViewModels
{
    public record UserWithTokenReadViewModel : UserReadViewModel
    {
        public string? Token { get; init; }
    }
}
