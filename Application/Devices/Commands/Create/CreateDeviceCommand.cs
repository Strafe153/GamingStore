using Application.Abstractions.MediatR;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Devices.Commands.Create;

public sealed record CreateDeviceCommand(
	string Name,
	DeviceCategory Category,
	decimal Price,
	int InStock,
	int CompanyId,
	IFormFile? Picture) : ICommand<Device>;