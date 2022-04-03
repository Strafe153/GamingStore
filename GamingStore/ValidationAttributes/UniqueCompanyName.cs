using System.ComponentModel.DataAnnotations;
using GamingStore.Models;
using GamingStore.Repositories.Interfaces;

namespace GamingStore.ValidationAttributes
{
    public class UniqueCompanyName : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            var service = (ICompanyControllable)context.GetService(typeof(ICompanyControllable))!;

            if (value is string name)
            {
                if (service.GetByNameAsync(name).Result is null)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult("The name is already taken");
            }

            return new ValidationResult("The value is of a wrong type");
        }
    }
}
