using System.ComponentModel.DataAnnotations;
using GamingStore.Data;
using GamingStore.ValidationAttributes;

namespace GamingStore.Dtos.Device
{
    public record DeviceUpdateDto
    {
        public DeviceCategory Category { get; init; } = DeviceCategory.Mouse;

        [Required]
        [Range(1, 1000)]
        public decimal Price { get; init; }

        [Required]
        [Range(1, 5000)]
        public int InStock { get; init; }

        public string Icon { get; init; } = string.Empty;
    }
}
