using Core.Dtos.UserDtos;
using FluentValidation;

namespace WebApi.Validators.UserValidators
{
    public class UserBaseValidator<T> : AbstractValidator<T> where T : UserBaseDto
    {
        public UserBaseValidator()
        {
            RuleFor(u => u.Username)
                .MinimumLength(2)
                .WithMessage("Username must be at least 2 characters long")
                .MaximumLength(30)
                .WithMessage("Username must not be longer than 30 characters");
        }
    }
}
