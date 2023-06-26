using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Devices.Commands.Delete;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Devices.Commands.Fixtures;

public class DeleteDeviceCommandHandlerFixture
{
    public DeleteDeviceCommandHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var deviceFaker = new Faker<Device>()
            .CustomInstantiator(f => new(
                f.Commerce.ProductName(),
                (DeviceCategory)Random.Shared.Next(Enum.GetValues(typeof(DeviceCategory)).Length),
                f.Random.Decimal(),
                f.Random.Int(),
                f.Internet.Url(),
                f.Random.Int(1, 5000)));

        var deleteDeviceCommandFaker = new Faker<DeleteDeviceCommand>()
            .CustomInstantiator(f => new(deviceFaker.Generate()));

        MockRepository = fixture.Freeze<Mock<IRepository<Device>>>();
        MockUnitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();
        MockPictureService = fixture.Freeze<Mock<IPictureService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<DeleteDeviceCommandHandler>>>();

        DeleteDeviceCommandHandler = new DeleteDeviceCommandHandler(
            MockRepository.Object,
            MockUnitOfWork.Object,
            MockPictureService.Object,
            MockLogger.Object);

        DeleteDeviceCommand = deleteDeviceCommandFaker.Generate();
    }

    public DeleteDeviceCommandHandler DeleteDeviceCommandHandler { get; }
    public Mock<IRepository<Device>> MockRepository { get; }
    public Mock<IUnitOfWork> MockUnitOfWork { get; }
    public Mock<IPictureService> MockPictureService { get; }
    public Mock<ILogger<DeleteDeviceCommandHandler>> MockLogger { get; }

    public CancellationToken CancellationToken { get; }
    public DeleteDeviceCommand DeleteDeviceCommand { get; }
}
