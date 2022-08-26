using Core.ViewModels.CompanyViewModels;
using FluentValidation;

namespace WebApi.Validators.CompanyValidators
{
    public class CompanyBaseValidator : AbstractValidator<CompanyBaseViewModel>
    {
        public CompanyBaseValidator()
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
}
