using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Fixtures;

public class DeviceServiceFixture
{
    public DeviceServiceFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockDeviceRepository = fixture.Freeze<Mock<IRepository<Device>>>();
        MockCacheService = fixture.Freeze<Mock<ICacheService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<DeviceService>>>();

        MockDeviceService = new(
            MockDeviceRepository.Object,
            MockCacheService.Object,
            MockLogger.Object);

        Id = 1;
        Name = "Name";
        Device = GetDevice();
        PaginatedList = GetPaginatedList();
    }

    public DeviceService MockDeviceService { get; }
    public Mock<IRepository<Device>> MockDeviceRepository { get; }
    public Mock<ICacheService> MockCacheService { get; }
    public Mock<ILogger<DeviceService>> MockLogger { get; }

    public int Id { get; }
    public string Name { get; }
    public Device Device { get; }
    public PaginatedList<Device> PaginatedList { get; }

    private Device GetDevice()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            Category = DeviceCategory.Mouse,
            Price = Id,
            InStock = Id,
            CompanyId = Id
        };
    }

    private List<Device> GetDevices()
    {
        return new()
        {
            Device,
            Device
        };
    }

    private PaginatedList<Device> GetPaginatedList()
    {
        return new(GetDevices(), 6, 1, 5);
    }
}
