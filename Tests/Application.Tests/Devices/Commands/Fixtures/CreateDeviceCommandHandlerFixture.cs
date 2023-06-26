using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Devices.Commands.Create;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Devices.Commands.Fixtures;

public class CreateDeviceCommandHandlerFixture
{
	public CreateDeviceCommandHandlerFixture()
	{
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var createDeviceCommandFaker = new Faker<CreateDeviceCommand>()
            .RuleFor(c => c.Name, f => f.Commerce.ProductName())
            .RuleFor(c => c.Category, f => (DeviceCategory)Random.Shared.Next(Enum.GetValues(typeof(DeviceCategory)).Length))
            .RuleFor(c => c.Price, f => f.Random.Decimal())
            .RuleFor(c => c.InStock, f => f.Random.Int())
            .RuleFor(c => c.CompanyId, f => f.Random.Int(1, 5000));

        MockRepository = fixture.Freeze<Mock<IRepository<Device>>>();
        MockUnitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();
        MockPictureService = fixture.Freeze<Mock<IPictureService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<CreateDeviceCommandHandler>>>();

        CreateDeviceCommandHandler = new CreateDeviceCommandHandler(
            MockRepository.Object,
            MockUnitOfWork.Object,
            MockPictureService.Object,
            MockLogger.Object);

        CreateDeviceCommand = createDeviceCommandFaker.Generate();
    }

    public CreateDeviceCommandHandler CreateDeviceCommandHandler { get; }
    public Mock<IRepository<Device>> MockRepository { get; }
    public Mock<IUnitOfWork> MockUnitOfWork { get; }
    public Mock<IPictureService> MockPictureService { get; }
    public Mock<ILogger<CreateDeviceCommandHandler>> MockLogger { get; }

    public CancellationToken CancellationToken { get; }
    public CreateDeviceCommand CreateDeviceCommand { get; }
}
