using Application.Abstractions.MediatR;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Devices.Commands.Create;

public sealed record CreateDeviceCommand : ICommand<Device>
{
    public string Name { get; init; } = default!;
    public DeviceCategory Category { get; init; }
    public decimal Price { get; init; }
    public int InStock { get; init; }
    public int CompanyId { get; init; }
    public IFormFile? Picture { get; init; }
}