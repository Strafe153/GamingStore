using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Devices.Commands.Create;

public sealed record CreateDeviceRequest(
    string Name,
    DeviceCategory Category,
    decimal Price,
    int InStock,
    int CompanyId,
    IFormFile? Picture);
