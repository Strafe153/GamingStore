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
				f.PickRandom<DeviceCategory>(),
				f.Random.Decimal(),
				f.Random.Int(),
				f.Internet.Url(),
				f.Random.Int(1, 5000)));

		var deleteDeviceCommandFaker = new Faker<DeleteDeviceCommand>()
			.CustomInstantiator(f => new(deviceFaker));

		MockDeviceRepository = fixture.Freeze<Mock<IRepository<Device>>>();
		MockDatabaseRepository = fixture.Freeze<Mock<IDatabaseRepository>>();
		MockPictureService = fixture.Freeze<Mock<IPictureService>>();
		MockLogger = fixture.Freeze<Mock<ILogger<DeleteDeviceCommandHandler>>>();

		DeleteDeviceCommandHandler = new DeleteDeviceCommandHandler(
			MockDeviceRepository.Object,
			MockDatabaseRepository.Object,
			MockPictureService.Object,
			MockLogger.Object);

		DeleteDeviceCommand = deleteDeviceCommandFaker.Generate();
	}

	public DeleteDeviceCommandHandler DeleteDeviceCommandHandler { get; }
	public Mock<IRepository<Device>> MockDeviceRepository { get; }
	public Mock<IDatabaseRepository> MockDatabaseRepository { get; }
	public Mock<IPictureService> MockPictureService { get; }
	public Mock<ILogger<DeleteDeviceCommandHandler>> MockLogger { get; }

	public CancellationToken CancellationToken { get; }
	public DeleteDeviceCommand DeleteDeviceCommand { get; }
}
