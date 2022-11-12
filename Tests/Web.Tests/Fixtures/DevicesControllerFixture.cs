using Application.Devices.Commands.Create;
using Application.Devices.Commands.Update;
using Application.Devices.Queries;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Presentation.Controllers;

namespace Presentation.Tests.Fixtures;

public class DevicesControllerFixture
{
	public DevicesControllerFixture()
	{
		var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockSender = fixture.Freeze<Mock<ISender>>();
        MockMapper = fixture.Freeze<Mock<IMapper>>();

        DevicesController = new DevicesController(
            MockSender.Object,
            MockMapper.Object);

        Id = 1;
        Name = "Name";
        PageParameters = GetPageParameters();
        Device = GetDevice();
        GetDeviceResponse = GetGetDeviceResponse();
        CreateDeviceRequest = GetCreateDeviceRequest();
        UpdateDeviceRequest = GetUpdateDeviceRequest();
        UpdateDeviceCommand = GetUpdateDeviceCommand();
        PaginatedList = GetPaginatedList();
        PaginatedModel = GetPaginatedModel();
    }

    public DevicesController DevicesController { get; }
    public Mock<ISender> MockSender { get; }
    public Mock<IMapper> MockMapper { get; }

    public int Id { get; }
    public string Name { get; }
    public IFormFile? Picture { get; }
    public Unit Unit { get; }
    public DevicePageParameters PageParameters { get; set; }
    public CancellationToken CancellationToken { get; }
    public Device Device { get; }
    public GetDeviceResponse GetDeviceResponse { get; }
    public CreateDeviceRequest CreateDeviceRequest { get; }
    public UpdateDeviceRequest UpdateDeviceRequest { get; }
    public UpdateDeviceCommand UpdateDeviceCommand { get; }
    public PaginatedList<Device> PaginatedList { get; }
    public PaginatedModel<GetDeviceResponse> PaginatedModel { get; }

    private DevicePageParameters GetPageParameters()
    {
        return new DevicePageParameters()
        {
            PageNumber = 1,
            PageSize = 5,
            Company = Name
        };
    }

    private Device GetDevice()
    {
        return new Device(Name, DeviceCategory.Mouse, 24.99M, 347, Name, 1);
    }

    private List<Device> GetDevices()
    {
        return new List<Device>()
        {
            Device,
            Device
        };
    }

    private GetDeviceResponse GetGetDeviceResponse()
    {
        return new GetDeviceResponse(Id, Name, DeviceCategory.Mouse, 24.99M, 347, Id, Name);
    }

    private List<GetDeviceResponse> GetDeviceResponses()
    {
        return new List<GetDeviceResponse>()
        {
            GetDeviceResponse,
            GetDeviceResponse
        };
    }

    private CreateDeviceRequest GetCreateDeviceRequest()
    {
        return new CreateDeviceRequest(Name, DeviceCategory.Mouse, 24.99M, 347, 1, Picture);
    }

    private UpdateDeviceRequest GetUpdateDeviceRequest()
    {
        return new UpdateDeviceRequest(Name, DeviceCategory.Mouse, 24.99M, 347, 1, Picture);
    }

    private UpdateDeviceCommand GetUpdateDeviceCommand()
    {
        return new UpdateDeviceCommand()
        {
            Device = Device,
            Name = Name,
            Category = DeviceCategory.Mat,
            Price = 24.99M,
            InStock = 347,
            CompanyId = 1,
            Picture = Picture
        };
    }

    private PaginatedList<Device> GetPaginatedList()
    {
        return new PaginatedList<Device>(GetDevices(), 5, 1, 5);
    }

    private PaginatedModel<GetDeviceResponse> GetPaginatedModel()
    {
        return new PaginatedModel<GetDeviceResponse>()
        {
            CurrentPage = 1,
            TotalPages = 2,
            PageSize = 5,
            TotalItems = 6,
            HasPrevious = false,
            HasNext = true,
            Entities = GetDeviceResponses()
        };
    }
}
