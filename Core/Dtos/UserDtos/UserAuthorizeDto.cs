namespace Core.Dtos.UserDtos
{
    public record UserAuthorizeDto : UserBaseDto
    {
        public string? Password { get; init; }
    }
}
