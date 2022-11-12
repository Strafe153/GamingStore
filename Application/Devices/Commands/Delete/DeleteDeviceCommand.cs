using Application.Abstractions.MediatR;
using Domain.Entities;

namespace Application.Devices.Commands.Delete;

public sealed record DeleteDeviceCommand(Device Device) : ICommand;
