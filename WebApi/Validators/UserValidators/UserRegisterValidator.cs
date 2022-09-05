using Core.Dtos.UserDtos;
using FluentValidation;

namespace WebApi.Validators.UserValidators
{
    public class UserRegisterValidator : UserBaseValidator<UserRegisterDto>
    {
        public UserRegisterValidator()
        {
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
