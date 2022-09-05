namespace Core.Dtos.UserDtos
{
    public record UserRegisterDto : UserBaseDto
    {
        public string? Password { get; init; }
    }
}
