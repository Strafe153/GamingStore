using Core.Dtos.UserDtos;
using FluentValidation;

namespace WebApi.Validators.UserValidators
{
    public class UserLoginValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginValidator()
        {
            RuleFor(u => u.Email)
                .MinimumLength(2)
                .WithMessage("Username must be at least 2 characters long")
                .MaximumLength(30)
                .WithMessage("Username must not be longer than 30 characters");

            RuleFor(p => p.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long")
                .MaximumLength(50)
                .WithMessage("Password must not be longer than 50 characters");
        }
    }
}
