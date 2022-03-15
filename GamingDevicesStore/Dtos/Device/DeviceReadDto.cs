using GamingDevicesStore.Data;

namespace GamingDevicesStore.Dtos.Device
{
    public record DeviceReadDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public DeviceCategory Category { get; init; }
        public decimal Price { get; init; }
        public Guid CompanyId { get; init; }
    }
}
