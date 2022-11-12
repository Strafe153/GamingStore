using FluentValidation;

namespace Application.Companies.Commands.Create;

public sealed class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
	public CreateCompanyCommandValidator()
	{
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Name is mandatory")
            .MinimumLength(2)
            .WithMessage("Name must be at least 2 characters long")
            .MaximumLength(50)
            .WithMessage("Name must not be longer than 50 characters");
    }
}
