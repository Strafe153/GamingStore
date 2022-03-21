using System.ComponentModel.DataAnnotations;
using GamingStore.Models;
using GamingStore.Repositories.Interfaces;

namespace GamingStore.ValidationAttributes
{
    public class UniqueUsername : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            var service = (IUserControllable)context.GetService(typeof(IUserControllable))!;

            if (value is string username)
            {
                if (service.GetByNameAsync(username).Result is null)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult("The username is already taken");
            }

            return new ValidationResult("The value is of a wrong type");
        }
    }
}
