using FluentValidation;

namespace Application.Users.Commands.ChangePassword;

public class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
{
	public ChangeUserPasswordCommandValidator()
	{
        RuleFor(u => u.CurrentPassword)
            .NotEmpty()
            .WithMessage("CurrentPassword is required")
            .MinimumLength(6)
            .WithMessage("CurrentPassword must be at least 6 characters long")
            .MaximumLength(24)
            .WithMessage("CurrentPassword must not be longer than 24 characters");

        RuleFor(u => u.NewPassword)
            .NotEmpty()
            .WithMessage("NewPassword is required")
            .MinimumLength(6)
            .WithMessage("NewPassword must be at least 6 characters long")
            .MaximumLength(24)
            .WithMessage("NewPassword must not be longer than 24 characters");
    }
}
