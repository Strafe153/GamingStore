using Application.Abstractions.MediatR;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Devices.Commands.Delete;

public sealed class DeleteDeviceCommandHandler : ICommandHandler<DeleteDeviceCommand>
{
	private readonly IRepository<Device> _deviceRepository;
	private readonly IDatabaseRepository _databaseRepository;
	private readonly IPictureService _pictureService;
	private readonly ILogger<DeleteDeviceCommandHandler> _logger;

	public DeleteDeviceCommandHandler(
		IRepository<Device> deviceRepository,
		IDatabaseRepository databaseRepository,
		IPictureService pictureService,
		ILogger<DeleteDeviceCommandHandler> logger)
	{
		_deviceRepository = deviceRepository;
		_databaseRepository = databaseRepository;
		_pictureService = pictureService;
		_logger = logger;
	}

	public async Task<Unit> Handle(DeleteDeviceCommand command, CancellationToken cancellationToken)
	{
		_deviceRepository.Delete(command.Device);
		await _databaseRepository.SaveChangesAsync();
		await _pictureService.DeleteAsync(command.Device.Picture!);

		_logger.LogInformation("Succesfully deleted a device with id {Id}", command.Device.Id);

		return Unit.Value;
	}
}
