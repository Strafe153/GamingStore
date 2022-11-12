using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Devices.Commands.Update;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Devices.Commands.Fixtures;

public class UpdateDeviceCommandHandlerFixture
{
    public UpdateDeviceCommandHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockRepository = fixture.Freeze<Mock<IRepository<Device>>>();
        MockUnitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();
        MockPictureService = fixture.Freeze<Mock<IPictureService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<UpdateDeviceCommandHandler>>>();

        UpdateDeviceCommandHandler = new UpdateDeviceCommandHandler(
            MockRepository.Object,
            MockUnitOfWork.Object,
            MockPictureService.Object,
            MockLogger.Object);

        Name = "Name";
        Device = GetDevice();
        UpdateDeviceCommand = GetUpdateDeviceCommand();
    }

    public UpdateDeviceCommandHandler UpdateDeviceCommandHandler { get; }
    public Mock<IRepository<Device>> MockRepository { get; }
    public Mock<IUnitOfWork> MockUnitOfWork { get; }
    public Mock<IPictureService> MockPictureService { get; }
    public Mock<ILogger<UpdateDeviceCommandHandler>> MockLogger { get; }

    public string Name { get; }
    public IFormFile? Picture { get; }
    public CancellationToken CancellationToken { get; }
    public Device Device { get; }
    public UpdateDeviceCommand UpdateDeviceCommand { get; }

    private Device GetDevice()
    {
        return new Device(Name, DeviceCategory.Mouse, 18.99M, 413, Name, 1);
    }

    private UpdateDeviceCommand GetUpdateDeviceCommand()
    {
        return new UpdateDeviceCommand()
        {
            Device = Device,
            Name = Name,
            Category = DeviceCategory.CableHolder,
            Price = 13.49M,
            InStock = 413,
            CompanyId = 3,
            Picture = Picture
        };
    }
}
