using System.ComponentModel.DataAnnotations;
using GamingStore.Data;
using GamingStore.ValidationAttributes;

namespace GamingStore.Dtos.Device
{
    public record DeviceCreateDto
    {
        [Required]
        [StringLength(40, MinimumLength = 1)]
        [UniqueDeviceName]
        public string Name { get; init; } = string.Empty;

        public DeviceCategory Category { get; init; } = DeviceCategory.Mouse;

        [Required]
        [Range(1, 1000)]
        public decimal Price { get; init; }

        [Required]
        [Range(1, 5000)]
        public int InStock { get; init; }

        [Required]
        public string Icon { get; init; } = string.Empty;

        [Required]
        public Guid CompanyId { get; init; }
    }
}
