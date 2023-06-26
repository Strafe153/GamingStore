using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Devices.Commands.Update;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Devices.Commands.Fixtures;

public class UpdateDeviceCommandHandlerFixture
{
    public UpdateDeviceCommandHandlerFixture()
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

        var updateDeviceCommandFaker = new Faker<UpdateDeviceCommand>()
            .RuleFor(c => c.Device, deviceFaker)
            .RuleFor(c => c.Name, f => f.Commerce.ProductName())
            .RuleFor(c => c.Category, f => (DeviceCategory)Random.Shared.Next(Enum.GetValues(typeof(DeviceCategory)).Length))
            .RuleFor(c => c.Price, f => f.Random.Decimal())
            .RuleFor(c => c.InStock, f => f.Random.Int())
            .RuleFor(c => c.CompanyId, f => f.Random.Int(1, 5000));

        MockRepository = fixture.Freeze<Mock<IRepository<Device>>>();
        MockUnitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();
        MockPictureService = fixture.Freeze<Mock<IPictureService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<UpdateDeviceCommandHandler>>>();

        UpdateDeviceCommandHandler = new UpdateDeviceCommandHandler(
            MockRepository.Object,
            MockUnitOfWork.Object,
            MockPictureService.Object,
            MockLogger.Object);

        UpdateDeviceCommand = updateDeviceCommandFaker.Generate();
    }

    public UpdateDeviceCommandHandler UpdateDeviceCommandHandler { get; }
    public Mock<IRepository<Device>> MockRepository { get; }
    public Mock<IUnitOfWork> MockUnitOfWork { get; }
    public Mock<IPictureService> MockPictureService { get; }
    public Mock<ILogger<UpdateDeviceCommandHandler>> MockLogger { get; }

    public CancellationToken CancellationToken { get; }
    public UpdateDeviceCommand UpdateDeviceCommand { get; }
}
