using GamingStore.Data;

namespace GamingStore.Dtos.Device
{
    public record DeviceReadDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public DeviceCategory Category { get; init; }
        public decimal Price { get; init; }
        public int InStock { get; init; }
        public Guid CompanyId { get; init; }
    }
}
