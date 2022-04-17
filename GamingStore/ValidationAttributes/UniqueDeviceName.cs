using System.ComponentModel.DataAnnotations;
using GamingStore.Models;
using GamingStore.Repositories.Interfaces;

namespace GamingStore.ValidationAttributes
{
    public class UniqueDeviceName : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            var service = (IControllable<Device>)context.GetService(typeof(IControllable<Device>))!;
            
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
