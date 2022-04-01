using System.ComponentModel.DataAnnotations;

namespace GamingStore.Dtos.Company
{
    public record CompanyCreateUpdateDto
    {
        [Required]
        [StringLength(25, MinimumLength = 1)]
        public string Name { get; init; } = string.Empty;
    }
}
