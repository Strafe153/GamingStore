using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Devices.Commands.Create;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Devices.Commands.Fixtures;

public class CreateDeviceCommandHandlerFixture
{
	public CreateDeviceCommandHandlerFixture()
	{
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockRepository = fixture.Freeze<Mock<IRepository<Device>>>();
        MockUnitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();
        MockPictureService = fixture.Freeze<Mock<IPictureService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<CreateDeviceCommandHandler>>>();

        CreateDeviceCommandHandler = new CreateDeviceCommandHandler(
            MockRepository.Object,
            MockUnitOfWork.Object,
            MockPictureService.Object,
            MockLogger.Object);

        Name = "Name";
        CreateDeviceCommand = GetCreateDeviceCommand();
    }

    public CreateDeviceCommandHandler CreateDeviceCommandHandler { get; }
    public Mock<IRepository<Device>> MockRepository { get; }
    public Mock<IUnitOfWork> MockUnitOfWork { get; }
    public Mock<IPictureService> MockPictureService { get; }
    public Mock<ILogger<CreateDeviceCommandHandler>> MockLogger { get; }

    public string Name { get; }
    public IFormFile? Picture { get; }
    public CancellationToken CancellationToken { get; }
    public CreateDeviceCommand CreateDeviceCommand { get; }

    private CreateDeviceCommand GetCreateDeviceCommand()
    {
        return new CreateDeviceCommand()
        {
            Name = Name,
            Category = DeviceCategory.Mouse,
            Price = 18.99M,
            InStock = 72,
            CompanyId = 1,
            Picture = Picture
        };
    }
}
