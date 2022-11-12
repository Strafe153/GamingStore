using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Companies.Queries.GetById;
using AutoFixture.AutoMoq;
using AutoFixture;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Application.Devices.Queries.GetById;
using Domain.Enums;

namespace Application.Tests.Devices.Queries.Fixtures;

public class GetDeviceByIdCommandHandlerFixture
{
    public GetDeviceByIdCommandHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockRepository = fixture.Freeze<Mock<IRepository<Device>>>();
        MockCacheService = fixture.Freeze<Mock<ICacheService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<GetDeviceByIdQueryHandler>>>();

        GetDeviceByIdQueryHandler = new GetDeviceByIdQueryHandler(
            MockRepository.Object,
            MockCacheService.Object,
            MockLogger.Object);

        Name = "Name";
        Device = GetDevice();
        GetDeviceByIdQuery = GetGetDeviceByIdQuery();
    }

    public GetDeviceByIdQueryHandler GetDeviceByIdQueryHandler { get; }
    public Mock<IRepository<Device>> MockRepository { get; }
    public Mock<ICacheService> MockCacheService { get; }
    public Mock<ILogger<GetDeviceByIdQueryHandler>> MockLogger { get; }

    public int Id { get; }
    public string Name { get; }
    public CancellationToken CancellationToken { get; }
    public Device Device { get; }
    public GetDeviceByIdQuery GetDeviceByIdQuery { get; }

    private GetDeviceByIdQuery GetGetDeviceByIdQuery()
    {
        return new GetDeviceByIdQuery(Id);
    }

    private Device GetDevice()
    {
        return new Device(Name, DeviceCategory.Gamepad, 99.99M, 4, Name, 2);
    }
}
