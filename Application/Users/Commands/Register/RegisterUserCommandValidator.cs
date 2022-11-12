using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Users.Commands.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(u => u.FirstName)
            .NotEmpty()
            .WithErrorCode("Username is required")
            .MinimumLength(2)
            .WithMessage("Username must be at least 2 characters long")
            .MaximumLength(30)
            .WithMessage("Username must not be longer than 30 characters")
            .Must(BeValidName)
            .WithMessage("The first letter must be capitalized");

        RuleFor(u => u.LastName)
            .NotEmpty()
            .WithErrorCode("Username is required")
            .MinimumLength(2)
            .WithMessage("Username must be at least 2 characters long")
            .MaximumLength(30)
            .WithMessage("Username must not be longer than 30 characters")
            .Must(BeValidName)
            .WithMessage("The first letter must be capitalized"); ;

        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Provide a valid email address");

        RuleFor(u => u.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .MinimumLength(10)
            .WithMessage("Phone number length must be at least 10 characters long")
            .MaximumLength(20)
            .WithMessage("Phone number length must be less than 20 characters")
            .Matches(new Regex(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$"))
            .WithMessage("Incorrect phone number format");

        RuleFor(p => p.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long")
            .MaximumLength(50)
            .WithMessage("Password must not be longer than 50 characters");
    }

    private bool BeValidName(string name)
    {
        var regex = new Regex(@"^[A-Z]{1}[a-z]*");
        return regex.IsMatch(name);
    }
}
