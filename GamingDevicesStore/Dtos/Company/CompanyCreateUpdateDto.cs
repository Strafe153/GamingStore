using System.ComponentModel.DataAnnotations;

namespace GamingDevicesStore.Dtos.Company
{
    public record CompanyCreateUpdateDto
    {
        [Required]
        [StringLength(25, MinimumLength = 1)]
        public string Name { get; init; } = string.Empty;
    }
}
