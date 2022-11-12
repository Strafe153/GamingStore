using Domain.Enums;

namespace Application.Devices.Queries;

public sealed record GetDeviceResponse(
    int Id,
    string Name,
    DeviceCategory Category,
    decimal Price,
    int InStock,
    int CompanyId,
    string? Picture);
