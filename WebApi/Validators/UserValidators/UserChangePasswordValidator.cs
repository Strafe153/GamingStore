using Core.ViewModels.UserViewModels;
using FluentValidation;

namespace WebApi.Validators.UserValidators
{
    public class UserChangePasswordValidator : AbstractValidator<UserChangePasswordViewModel>
    {
        public UserChangePasswordValidator()
        {
            RuleFor(u => u.Password)
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long")
                .MaximumLength(50)
                .WithMessage("Password must not be longer than 50 characters");
        }
    }
}
