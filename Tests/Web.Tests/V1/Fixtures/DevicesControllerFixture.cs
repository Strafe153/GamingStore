using Application.AutoMapperProfiles;
using Application.Devices.Commands.Create;
using Application.Devices.Commands.Update;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using Domain.Shared.PageParameters;
using Domain.Shared.Paging;
using MediatR;
using Moq;
using Web.Controllers.V1;

namespace Web.Tests.V1.Fixtures;

public class DevicesControllerFixture
{
    public DevicesControllerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var deviceFaker = new Faker<Device>()
            .CustomInstantiator(f => new(
                f.Commerce.ProductName(),
                (DeviceCategory)Random.Shared.Next(Enum.GetValues(typeof(DeviceCategory)).Length),
                f.Random.Decimal(),
                f.Random.Int(),
                f.Internet.Url(),
                f.Random.Int()));

        var createDeviceRequestFaker = new Faker<CreateDeviceRequest>()
            .CustomInstantiator(f => new(
                f.Commerce.ProductName(),
                (DeviceCategory)Random.Shared.Next(Enum.GetValues(typeof(DeviceCategory)).Length),
                f.Random.Decimal(),
                f.Random.Int(),
                f.Random.Int(),
                null));

        var updateDeviceRequestFaker = new Faker<UpdateDeviceRequest>()
            .CustomInstantiator(f => new(
                f.Commerce.ProductName(),
                (DeviceCategory)Random.Shared.Next(Enum.GetValues(typeof(DeviceCategory)).Length),
                f.Random.Decimal(),
                f.Random.Int(),
                f.Random.Int(),
                null));

        var totalItemsCount = Random.Shared.Next(2, 50);

        var pagedListFaker = new Faker<PagedList<Device>>()
            .CustomInstantiator(f => new(
                deviceFaker.Generate(totalItemsCount),
                totalItemsCount,
                f.Random.Int(1, 2),
                f.Random.Int(1, 2)))
            .RuleFor(l => l.PageSize, (f, l) => f.Random.Int(1, l.TotalItems))
            .RuleFor(l => l.CurrentPage, (f, l) => f.Random.Int(1, l.TotalPages));

        MockSender = fixture.Freeze<Mock<ISender>>();

        Mapper = new MapperConfiguration(options =>
        {
            options.AddProfile(new DeviceProfile());
        }).CreateMapper();

        DevicesController = new DevicesController(
            MockSender.Object,
            Mapper);

        Id = Random.Shared.Next(1, 5000);

        PageParameters = new()
        {
            PageNumber = Random.Shared.Next(1, 500),
            PageSize = Random.Shared.Next(1, 500)
        };

        Device = deviceFaker.Generate();
        CreateDeviceRequest = createDeviceRequestFaker.Generate();
        UpdateDeviceRequest = updateDeviceRequestFaker.Generate();
        PagedList = pagedListFaker.Generate();
    }

    public DevicesController DevicesController { get; }
    public Mock<ISender> MockSender { get; }
    public IMapper Mapper { get; }

    public int Id { get; }
    public DeviceParameters PageParameters { get; set; }
    public CancellationToken CancellationToken { get; }
    public Device Device { get; }
    public CreateDeviceRequest CreateDeviceRequest { get; }
    public UpdateDeviceRequest UpdateDeviceRequest { get; }
    public PagedList<Device> PagedList { get; }
}
