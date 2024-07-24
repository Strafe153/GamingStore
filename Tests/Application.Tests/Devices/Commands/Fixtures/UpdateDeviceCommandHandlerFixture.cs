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
				f.PickRandom<DeviceCategory>(),
				f.Random.Decimal(),
				f.Random.Int(),
				f.Internet.Url(),
				f.Random.Int(1, 5000)));

		var updateDeviceCommandFaker = new Faker<UpdateDeviceCommand>()
			.CustomInstantiator(f => new(
				deviceFaker,
				f.Commerce.ProductName(),
				f.PickRandom<DeviceCategory>(),
				f.Random.Decimal(),
				f.Random.Int(),
				f.Random.Int(1, 5000),
				null));

		MockDeviceRepository = fixture.Freeze<Mock<IRepository<Device>>>();
		MockDatabaseRepository = fixture.Freeze<Mock<IDatabaseRepository>>();
		MockPictureService = fixture.Freeze<Mock<IPictureService>>();
		MockLogger = fixture.Freeze<Mock<ILogger<UpdateDeviceCommandHandler>>>();

		UpdateDeviceCommandHandler = new UpdateDeviceCommandHandler(
			MockDeviceRepository.Object,
			MockDatabaseRepository.Object,
			MockPictureService.Object,
			MockLogger.Object);

		UpdateDeviceCommand = updateDeviceCommandFaker.Generate();
	}

	public UpdateDeviceCommandHandler UpdateDeviceCommandHandler { get; }
	public Mock<IRepository<Device>> MockDeviceRepository { get; }
	public Mock<IDatabaseRepository> MockDatabaseRepository { get; }
	public Mock<IPictureService> MockPictureService { get; }
	public Mock<ILogger<UpdateDeviceCommandHandler>> MockLogger { get; }

	public CancellationToken CancellationToken { get; }
	public UpdateDeviceCommand UpdateDeviceCommand { get; }
}
