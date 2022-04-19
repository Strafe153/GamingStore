using System.ComponentModel.DataAnnotations;
using GamingStore.ValidationAttributes;

namespace GamingStore.Dtos.Company
{
    public record CompanyCreateDto
    {
        [Required]
        [StringLength(25, MinimumLength = 1)]
        [UniqueCompanyName]
        public string Name { get; init; } = string.Empty;

        [Required]
        public string Icon { get; init; } = string.Empty;
    }
}
