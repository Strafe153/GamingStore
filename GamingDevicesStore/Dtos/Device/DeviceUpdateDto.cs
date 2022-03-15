using System.ComponentModel.DataAnnotations;
using GamingDevicesStore.Data;

namespace GamingDevicesStore.Dtos.Device
{
    public record DeviceUpdateDto
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Name { get; init; } = string.Empty;

        public DeviceCategory Category { get; init; } = DeviceCategory.Mouse;

        [Required]
        [Range(1, 1000)]
        public decimal Price { get; init; }
    }
}
