using Core.Enums;
using Core.Dtos.UserDtos;
using FluentValidation;

namespace WebApi.Validators.UserValidators
{
    public class UserChangeRoleValidator : AbstractValidator<UserChangeRoleDto>
    {
        public UserChangeRoleValidator()
        {
            RuleFor(p => p.Role)
                .Must(BeInRange)
                .WithMessage("Role must be in the range from 0 to 1 inclusive");
        }

        private bool BeInRange(UserRole role)
        {
            int roleAsInt = (int)role;

            if ((roleAsInt > -1) && (roleAsInt < 2))
            {
                return true;
            }

            return false;
        }
    }
}
