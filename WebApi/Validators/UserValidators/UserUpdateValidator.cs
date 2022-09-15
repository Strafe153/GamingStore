using Core.Dtos.UserDtos;
using FluentValidation;

namespace WebApi.Validators.UserValidators
{
    public class UserUpdateValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty()
                .WithMessage("Username is required")
                .MinimumLength(2)
                .WithMessage("Username must be at least 2 characters long")
                .MaximumLength(30)
                .WithMessage("Username must not be longer than 30 characters");
        }
    }
}
