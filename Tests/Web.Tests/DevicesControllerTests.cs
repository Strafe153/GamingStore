using Application.Devices.Commands.Create;
using Application.Devices.Commands.Update;
using Application.Devices.Queries;
using Application.Devices.Queries.GetAll;
using Application.Devices.Queries.GetById;
using Domain.Entities;
using Domain.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Tests.Fixtures;
using Xunit;

namespace Presentation.Tests;

public class DevicesControllerTests : IClassFixture<DevicesControllerFixture>
{
    private readonly DevicesControllerFixture _fixture;

	public DevicesControllerTests(DevicesControllerFixture fixture)
	{
		_fixture = fixture;
	}

    [Fact]
    public async Task GetAsync_Should_ReturnActionResultOfPaginatedModelOfGetDeviceResponse_WhenDataIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetAllDevicesQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.PaginatedList);

        _fixture.MockMapper
            .Setup(m => m.Map<PaginatedModel<GetDeviceResponse>>(It.IsAny<PaginatedList<Device>>()))
            .Returns(_fixture.PaginatedModel);

        // Act
        var result = await _fixture.DevicesController.GetAsync(_fixture.PageParameters, _fixture.CancellationToken);
        var objectResult = result.Result.As<OkObjectResult>();
        var paginatedModel = objectResult.Value.As<PaginatedModel<GetDeviceResponse>>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<PaginatedModel<GetDeviceResponse>>>();
        objectResult.StatusCode.Should().Be(200);
        paginatedModel.Entities.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAsync_ShouldReturnActionResultOfGetDeviceResponse_WhenDeviceExists()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetDeviceByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Device);

        _fixture.MockMapper
            .Setup(m => m.Map<GetDeviceResponse>(It.IsAny<Device>()))
            .Returns(_fixture.GetDeviceResponse);

        // Act
        var result = await _fixture.DevicesController.GetAsync(_fixture.Id, _fixture.CancellationToken);
        var objectResult = result.Result.As<OkObjectResult>();
        var getCompanyResponse = objectResult.Value.As<GetDeviceResponse>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<GetDeviceResponse>>();
        objectResult.StatusCode.Should().Be(200);
        getCompanyResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnActionResultOfGetDeviceResponse_WhenRequestIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<CreateDeviceCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Device);

        _fixture.MockMapper
            .Setup(m => m.Map<GetDeviceResponse>(It.IsAny<Device>()))
            .Returns(_fixture.GetDeviceResponse);

        // Act
        var result = await _fixture.DevicesController.CreateAsync(_fixture.CreateDeviceRequest, _fixture.CancellationToken);
        var objectResult = result.Result.As<CreatedAtActionResult>();
        var getCompanyResponse = objectResult.Value.As<GetDeviceResponse>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<GetDeviceResponse>>();
        objectResult.StatusCode.Should().Be(201);
        getCompanyResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_ReturnNoContentResult_WhenDeviceExistsAndRequestIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetDeviceByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Device);

        _fixture.MockMapper
            .Setup(m => m.Map<UpdateDeviceCommand>(It.IsAny<UpdateDeviceRequest>()))
            .Returns(_fixture.UpdateDeviceCommand);

        // Act
        var result = await _fixture.DevicesController
            .UpdateAsync(_fixture.Id, _fixture.UpdateDeviceRequest, _fixture.CancellationToken);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task DeleteAsync_Should_ReturnNoContentResult_WhenDeviceExists()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetDeviceByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Device);

        // Act
        var result = await _fixture.DevicesController.DeleteAsync(_fixture.Id, _fixture.CancellationToken);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.Should().NotBeNull();
    }
}
