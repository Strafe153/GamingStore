﻿using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Devices.Queries.GetById;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Devices.Queries.Fixtures;

public class GetDeviceByIdCommandHandlerFixture
{
	public GetDeviceByIdCommandHandlerFixture()
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

		var getDeviceByIdQueryFaker = new Faker<GetDeviceByIdQuery>()
			.CustomInstantiator(f => new(f.Random.Int(1, 5000)));

		MockRepository = fixture.Freeze<Mock<IRepository<Device>>>();
		MockCacheService = fixture.Freeze<Mock<ICacheService>>();
		MockLogger = fixture.Freeze<Mock<ILogger<GetDeviceByIdQueryHandler>>>();

		GetDeviceByIdQueryHandler = new GetDeviceByIdQueryHandler(
			MockRepository.Object,
			MockCacheService.Object,
			MockLogger.Object);

		Device = deviceFaker.Generate();
		GetDeviceByIdQuery = getDeviceByIdQueryFaker.Generate();
	}

	public GetDeviceByIdQueryHandler GetDeviceByIdQueryHandler { get; }
	public Mock<IRepository<Device>> MockRepository { get; }
	public Mock<ICacheService> MockCacheService { get; }
	public Mock<ILogger<GetDeviceByIdQueryHandler>> MockLogger { get; }

	public CancellationToken CancellationToken { get; }
	public Device Device { get; set; }
	public GetDeviceByIdQuery GetDeviceByIdQuery { get; }
}
