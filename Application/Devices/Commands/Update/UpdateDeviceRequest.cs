using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Devices.Commands.Update;

public sealed record UpdateDeviceRequest(
    string Name,
    DeviceCategory Category,
    decimal Price,
    int InStock,
    int CompanyId,
    IFormFile? Picture);
