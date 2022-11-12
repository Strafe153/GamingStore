using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Companies.Commands.Delete;
using AutoFixture.AutoMoq;
using AutoFixture;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Application.Devices.Commands.Delete;
using Domain.Enums;

namespace Application.Tests.Devices.Commands.Fixtures;

public class DeleteDeviceCommandHandlerFixture
{
    public DeleteDeviceCommandHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockRepository = fixture.Freeze<Mock<IRepository<Device>>>();
        MockUnitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();
        MockPictureService = fixture.Freeze<Mock<IPictureService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<DeleteDeviceCommandHandler>>>();

        DeleteDeviceCommandHandler = new DeleteDeviceCommandHandler(
            MockRepository.Object,
            MockUnitOfWork.Object,
            MockPictureService.Object,
            MockLogger.Object);

        Name = "Name";
        Device = GetDevice();
        DeleteDeviceCommand = GetDeleteDeviceCommand();
    }
    public DeleteDeviceCommandHandler DeleteDeviceCommandHandler { get; }
    public Mock<IRepository<Device>> MockRepository { get; }
    public Mock<IUnitOfWork> MockUnitOfWork { get; }
    public Mock<IPictureService> MockPictureService { get; }
    public Mock<ILogger<DeleteDeviceCommandHandler>> MockLogger { get; }

    public string Name { get; }
    public CancellationToken CancellationToken { get; }
    public Device Device { get; }
    public DeleteDeviceCommand DeleteDeviceCommand { get; }

    private Device GetDevice()
    {
        return new Device(Name, DeviceCategory.Mat, 6.99M, 501, Name, 1);
    }

    private DeleteDeviceCommand GetDeleteDeviceCommand()
    {
        return new DeleteDeviceCommand(Device);
    }
}
