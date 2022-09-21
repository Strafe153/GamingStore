using Core.Enums;
using Core.Dtos.DeviceDtos;
using FluentValidation;

namespace WebApi.Validators.DeviceValidators;

public class DeviceBaseValidator : AbstractValidator<DeviceBaseDto>
{
    public DeviceBaseValidator()
    {
        RuleFor(d => d.Name)
            .NotEmpty()
            .WithMessage("Name is mandatory")
            .MinimumLength(2)
            .WithMessage("Name must be at least 2 characters long")
            .MaximumLength(50)
            .WithMessage("Name must not be longer than 50 characters");

        RuleFor(d => d.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0")
            .LessThan(1001)
            .WithMessage("Price must not be greater than 1000");

        RuleFor(d => d.InStock)
            .GreaterThan(0)
            .WithMessage("InStock must be greater than 0")
            .LessThan(5001)
            .WithMessage("InStock must not be greater than 5000");

        RuleFor(d => d.Category)
            .NotEmpty()
            .WithMessage("Category is mandatory")
            .Must(BeInRange)
            .WithMessage("Category must be in the range from 0 to 7 inclusive");

        RuleFor(d => d.CompanyId)
            .NotEmpty()
            .WithMessage("CompanyId is mandatory");
    }

    private bool BeInRange(DeviceCategory category)
    {
        int typeAsInt = (int)category;
        int totalCategories = Enum.GetNames(typeof(DeviceCategory)).Length;

        if ((typeAsInt > -1) && (typeAsInt < totalCategories))
        {
            return true;
        }

        return false;
    }
}
