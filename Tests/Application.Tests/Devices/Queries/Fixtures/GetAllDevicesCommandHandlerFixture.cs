using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Devices.Queries.GetAll;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using Domain.Shared.Paging;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Devices.Queries.Fixtures;

public class GetAllDevicesCommandHandlerFixture
{
    public GetAllDevicesCommandHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var getAllDevicesQueryFaker = new Faker<GetAllDevicesQuery>()
            .CustomInstantiator(f => new(
                f.Random.Int(1, 500),
                f.Random.Int(1, 500),
                f.Company.CompanyName()));

        var deviceFaker = new Faker<Device>()
            .CustomInstantiator(f => new(
                f.Commerce.ProductName(),
                (DeviceCategory)Random.Shared.Next(Enum.GetValues(typeof(DeviceCategory)).Length),
                f.Random.Decimal(),
                f.Random.Int(),
                f.Internet.Url(),
                f.Random.Int(1, 5000)));

        var totalItemsCount = Random.Shared.Next(2, 50);

        var paginatedListFaker = new Faker<PaginatedList<Device>>()
            .CustomInstantiator(f => new(
                deviceFaker.Generate(totalItemsCount),
                totalItemsCount,
                f.Random.Int(1, 2),
                f.Random.Int(1, 2)))
            .RuleFor(l => l.PageSize, (f, l) => f.Random.Int(1, l.TotalItems))
            .RuleFor(l => l.CurrentPage, (f, l) => f.Random.Int(1, l.TotalPages));

        MockRepository = fixture.Freeze<Mock<IRepository<Device>>>();
        MockCacheService = fixture.Freeze<Mock<ICacheService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<GetAllDevicesQueryHandler>>>();

        GetAllDevicesQueryHandler = new GetAllDevicesQueryHandler(
            MockRepository.Object,
            MockCacheService.Object,
            MockLogger.Object);

        GetAllDevicesQuery = getAllDevicesQueryFaker.Generate();
        PaginatedList = paginatedListFaker.Generate();
    }

    public GetAllDevicesQueryHandler GetAllDevicesQueryHandler { get; }
    public Mock<IRepository<Device>> MockRepository { get; }
    public Mock<ICacheService> MockCacheService { get; }
    public Mock<ILogger<GetAllDevicesQueryHandler>> MockLogger { get; }

    public CancellationToken CancellationToken { get; }
    public GetAllDevicesQuery GetAllDevicesQuery { get; }
    public PaginatedList<Device> PaginatedList { get; }
}
