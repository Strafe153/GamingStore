namespace Core.ViewModels.UserViewModels
{
    public record UserAuthorizeViewModel : UserBaseViewModel
    {
        public string? Password { get; init; }
    }
}
