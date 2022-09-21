using Core.Enums;

namespace Core.Dtos.DeviceDtos;

public record DeviceBaseDto
{
    public string? Name { get; init; }
    public DeviceCategory Category { get; init; }
    public decimal Price { get; init; }
    public int InStock { get; init; }
    public int CompanyId { get; init; }
}
