using Application.Abstractions.MediatR;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Devices.Commands.Create;

public sealed class CreateDeviceCommandHandler : ICommandHandler<CreateDeviceCommand, Device>
{
    private readonly IRepository<Device> _deviceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPictureService _pictureService;
    private readonly ILogger<CreateDeviceCommandHandler> _logger;

    public CreateDeviceCommandHandler(
        IRepository<Device> deviceRepository, 
        IUnitOfWork unitOfWork, 
        IPictureService pictureService,
        ILogger<CreateDeviceCommandHandler> logger)
    {
        _deviceRepository = deviceRepository;
        _unitOfWork = unitOfWork;
        _pictureService = pictureService;
        _logger = logger;
    }

    public async Task<Device> Handle(CreateDeviceCommand command, CancellationToken cancellationToken)
    {
        var picture = await _pictureService.UploadAsync(command.Picture, "device-pictures", command.Name);
        var device = new Device(
            command.Name,
            command.Category,
            command.Price,
            command.InStock,
            picture,
            command.CompanyId);

        try
        {
            _deviceRepository.Create(device);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Succesfully created a Device");

            return device;
        }
        catch (DbUpdateException)
        {
            _logger.LogWarning("Failed to create a Device. The name '{Name}' is already taken", command.Name);
            throw new ValueNotUniqueException($"Name '{command.Name}' is already taken");
        }
    }
}
