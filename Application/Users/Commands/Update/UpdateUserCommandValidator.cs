using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Users.Commands.Update;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
	public UpdateUserCommandValidator()
	{
		RuleFor(u => u.FirstName)
			.NotEmpty()
			.WithErrorCode("First name is required")
			.MinimumLength(2)
			.WithMessage("First name must be at least 2 characters long")
			.MaximumLength(30)
			.WithMessage("First name must not be longer than 30 characters")
			.Must(BeValidName)
			.WithMessage("The first letter must be capitalized");

		RuleFor(u => u.LastName)
			.NotEmpty()
			.WithErrorCode("Last name is required")
			.MinimumLength(2)
			.WithMessage("Last name must be at least 2 characters long")
			.MaximumLength(30)
			.WithMessage("Last name must not be longer than 30 characters")
			.Must(BeValidName)
			.WithMessage("The first letter must be capitalized");

		RuleFor(u => u.PhoneNumber)
			.NotEmpty()
			.WithMessage("Phone number is required")
			.MinimumLength(10)
			.WithMessage("Phone number length must be at least 10 characters long")
			.MaximumLength(20)
			.WithMessage("Phone number length must be less than 20 characters")
			.Matches(new Regex(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$"))
			.WithMessage("Incorrect phone number format");
	}

	private bool BeValidName(string name) => new Regex(@"^[A-Z]{1}[a-z]*").IsMatch(name);
}
