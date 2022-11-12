using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Companies.Queries.GetAll;
using Application.Devices.Queries.GetAll;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Domain.Enums;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Devices.Queries.Fixtures;

public class GetAllDevicesCommandHandlerFixture
{
    public GetAllDevicesCommandHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockRepository = fixture.Freeze<Mock<IRepository<Device>>>();
        MockCacheService = fixture.Freeze<Mock<ICacheService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<GetAllDevicesQueryHandler>>>();

        GetAllDevicesQueryHandler = new GetAllDevicesQueryHandler(
            MockRepository.Object,
            MockCacheService.Object,
            MockLogger.Object);

        Name = "Name";
        GetAllDevicesQuery = GetGetAllDevicesQuery();
        PaginatedList = GetPaginatedList();
    }

    public GetAllDevicesQueryHandler GetAllDevicesQueryHandler { get; }
    public Mock<IRepository<Device>> MockRepository { get; }
    public Mock<ICacheService> MockCacheService { get; }
    public Mock<ILogger<GetAllDevicesQueryHandler>> MockLogger { get; }

    public string Name { get; }
    public CancellationToken CancellationToken { get; }
    public GetAllDevicesQuery GetAllDevicesQuery { get; }
    public PaginatedList<Device> PaginatedList { get; }

    private GetAllDevicesQuery GetGetAllDevicesQuery()
    {
        return new GetAllDevicesQuery(1, 5, Name);
    }

    private Device GetDevice()
    {
        return new Device(Name, DeviceCategory.Earphones, 64.99M, 123, Name, 1);
    }

    private List<Device> GetDevices()
    {
        return new List<Device>()
        {
            GetDevice(),
            GetDevice()
        };
    }

    private PaginatedList<Device> GetPaginatedList()
    {
        return new PaginatedList<Device>(GetDevices(), 6, 1, 5);
    }
}
