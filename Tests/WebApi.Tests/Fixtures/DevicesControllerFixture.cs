using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Core.Dtos;
using Core.Dtos.DeviceDtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using WebApi.Controllers;

namespace WebApi.Tests.Fixtures;

public class DevicesControllerFixture
{
    public DevicesControllerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockDeviceService = fixture.Freeze<Mock<IDeviceService>>();
        MockPictureService = fixture.Freeze<Mock<IPictureService>>();
        MockMapper = fixture.Freeze<Mock<IMapper>>();

        MockDevicesController = new DevicesController(
            MockDeviceService.Object,
            MockPictureService.Object,
            MockMapper.Object);

        Id = 1;
        Name = "Device";
        Device = GetDevice();
        DeviceBaseDto = GetDeviceBaseDto();
        DeviceReadDto = GetDeviceReadDto();
        DeviceCreateUpdateDto = GetDeviceCreateUpdateDto();
        PageParameters = GetPageParameters();
        DevicePaginatedList = GetDevicePaginatedList();
        DevicePageDto = GetDevicePageDto();
    }

    public DevicesController MockDevicesController { get; }
    public Mock<IDeviceService> MockDeviceService { get; }
    public Mock<IPictureService> MockPictureService { get; }
    public Mock<IMapper> MockMapper { get; }

    public int Id { get; }
    public string Name { get; }
    public IFormFile? Picture { get; }
    public Device Device { get; }
    public DeviceReadDto DeviceReadDto { get; }
    public DeviceBaseDto DeviceBaseDto { get; }
    public DeviceCreateUpdateDto DeviceCreateUpdateDto { get; }
    public DevicePageParameters PageParameters { get; }
    public PaginatedList<Device> DevicePaginatedList { get; }
    public PageDto<DeviceReadDto> DevicePageDto { get; }
    public CancellationToken CancellationToken { get; }

    private Device GetDevice()
    {
        return new Device()
        {
            Id = Id,
            Name = Name,
            Category = DeviceCategory.Mouse,
            InStock = 500,
            Price = 49.99M,
            CompanyId = Id
        };
    }

    private List<Device> GetDevices()
    {
        return new List<Device>()
        {
            GetDevice(),
            GetDevice()
        };
    }

    private DevicePageParameters GetPageParameters()
    {
        return new DevicePageParameters()
        {
            PageNumber = 1,
            PageSize = 5,
            Company = Name
        };
    }

    private PaginatedList<Device> GetDevicePaginatedList()
    {
        return new PaginatedList<Device>(GetDevices(), 6, 1, 5);
    }

    private DeviceReadDto GetDeviceReadDto()
    {
        return new DeviceReadDto()
        {
            Id = Id,
            Name = Name,
            Category = DeviceCategory.Mouse,
            InStock = 500,
            Price = 49.99M,
            CompanyId = Id
        };
    }

    private DeviceBaseDto GetDeviceBaseDto()
    {
        return new DeviceBaseDto()
        {
            Name = Name,
            Category = DeviceCategory.Mouse,
            InStock = 500,
            Price = 49.99M,
            CompanyId = Id
        };
    }

    private DeviceCreateUpdateDto GetDeviceCreateUpdateDto()
    {
        return new DeviceCreateUpdateDto()
        {
            Name = Name,
            Category = DeviceCategory.Mouse,
            Price = Id,
            InStock = Id,
            CompanyId = Id,
            Picture = Picture
        };
    }

    private List<DeviceReadDto> GetDeviceReadDtos()
    {
        return new List<DeviceReadDto>()
        {
            DeviceReadDto,
            DeviceReadDto
        };
    }

    private PageDto<DeviceReadDto> GetDevicePageDto()
    {
        return new PageDto<DeviceReadDto>()
        {
            CurrentPage = 1,
            TotalPages = 2,
            PageSize = 5,
            TotalItems = 6,
            HasPrevious = false,
            HasNext = true,
            Entities = GetDeviceReadDtos()
        };
    }
}
