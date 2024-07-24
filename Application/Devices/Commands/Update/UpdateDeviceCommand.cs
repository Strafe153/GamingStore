using Application.Abstractions.MediatR;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Devices.Commands.Update;

public sealed record UpdateDeviceCommand(
	Device Device,
	string Name,
	DeviceCategory Category,
	decimal Price,
	int InStock,
	int CompanyId,
	IFormFile? Picture) : ICommand;