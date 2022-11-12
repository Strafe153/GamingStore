using Application.Abstractions.MediatR;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Devices.Commands.Update;

public sealed class UpdateDeviceCommandHandler : ICommandHandler<UpdateDeviceCommand>
{
    private readonly IRepository<Device> _deviceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPictureService _pictureService;
    private readonly ILogger<UpdateDeviceCommandHandler> _logger;

    public UpdateDeviceCommandHandler(
        IRepository<Device> deviceRepository, 
        IUnitOfWork unitOfWork, 
        IPictureService pictureService,
        ILogger<UpdateDeviceCommandHandler> logger)
    {
        _deviceRepository = deviceRepository;
        _unitOfWork = unitOfWork;
        _pictureService = pictureService;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateDeviceCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await _pictureService.DeleteAsync(command.Device.Picture);

            command.Device.Name = command.Name;
            command.Device.Category = command.Category;
            command.Device.Price = command.Price;
            command.Device.InStock = command.InStock;
            command.Device.CompanyId = command.CompanyId;
            command.Device.Picture = await _pictureService.UploadAsync(command.Picture, "device-pictures", command.Name);

            _deviceRepository.Update(command.Device);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully updated a device with id {Id}", command.Device.Id);

            return Unit.Value;
        }
        catch (DbUpdateException)
        {
            _logger.LogWarning("Failed to update the device wiht id {Id}. The name '{Name}' is already taken",
                command.Device.Id, command.Device.Name);
            throw new ValueNotUniqueException($"Name '{command.Device.Name}' is already taken");
        }
    }
}
