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
			.CustomInstantiator(f => new(
				f.Commerce.ProductName(),
				f.PickRandom<DeviceCategory>(),
				f.Random.Decimal(),
				f.Random.Int(),
				f.Random.Int(1, 5000),
				null));

		MockDeviceRepository = fixture.Freeze<Mock<IRepository<Device>>>();
		MockDatabaseRepository = fixture.Freeze<Mock<IDatabaseRepository>>();
		MockPictureService = fixture.Freeze<Mock<IPictureService>>();
		MockLogger = fixture.Freeze<Mock<ILogger<CreateDeviceCommandHandler>>>();

		CreateDeviceCommandHandler = new CreateDeviceCommandHandler(
			MockDeviceRepository.Object,
			MockDatabaseRepository.Object,
			MockPictureService.Object,
			MockLogger.Object);

		CreateDeviceCommand = createDeviceCommandFaker.Generate();
	}

	public CreateDeviceCommandHandler CreateDeviceCommandHandler { get; }
	public Mock<IRepository<Device>> MockDeviceRepository { get; }
	public Mock<IDatabaseRepository> MockDatabaseRepository { get; }
	public Mock<IPictureService> MockPictureService { get; }
	public Mock<ILogger<CreateDeviceCommandHandler>> MockLogger { get; }

	public CancellationToken CancellationToken { get; }
	public CreateDeviceCommand CreateDeviceCommand { get; }
}
