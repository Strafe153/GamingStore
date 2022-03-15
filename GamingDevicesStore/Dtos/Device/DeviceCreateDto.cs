using System.ComponentModel.DataAnnotations;
using GamingDevicesStore.Data;

namespace GamingDevicesStore.Dtos.Device
{
    public record DeviceCreateDto
    {
        [Required]
        [StringLength(40, MinimumLength = 1)]
        public string Name { get; init; } = string.Empty;

        public DeviceCategory Category { get; init; } = DeviceCategory.Mouse;

        [Required]
        [Range(1, 1000)]
        public decimal Price { get; init; }

        [Required]
        public Guid CompanyId { get; init; }
    }
}
