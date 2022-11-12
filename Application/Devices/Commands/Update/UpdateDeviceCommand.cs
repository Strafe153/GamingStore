using Application.Abstractions.MediatR;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Devices.Commands.Update;

public sealed record UpdateDeviceCommand : ICommand
{
    public Device Device { get; init; } = default!;
    public string Name { get; init; } = default!;
    public DeviceCategory Category { get; init; }
    public decimal Price { get; init; }
    public int InStock { get; init; }
    public int CompanyId { get; init; }
    public IFormFile? Picture { get; init; }
}
