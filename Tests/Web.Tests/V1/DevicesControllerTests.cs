using Application.Devices.Commands.Create;
using Application.Devices.Queries;
using Application.Devices.Queries.GetAll;
using Application.Devices.Queries.GetById;
using Domain.Shared.Paging;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Tests.V1.Fixtures;
using Xunit;

namespace Web.Tests.V1;

public class DevicesControllerTests : IClassFixture<DevicesControllerFixture>
{
    private readonly DevicesControllerFixture _fixture;

    public DevicesControllerTests(DevicesControllerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Get_Should_ReturnActionResultOfPagedModelOfGetDeviceResponse_WhenDataIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetAllDevicesQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.PagedList);

        // Act
        var result = await _fixture.DevicesController.Get(_fixture.PageParameters, _fixture.CancellationToken);
        var objectResult = result.Result.As<OkObjectResult>();
        var pagedModel = objectResult.Value.As<PagedModel<GetDeviceResponse>>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<PagedModel<GetDeviceResponse>>>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        pagedModel.Entities.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Get_ShouldReturnActionResultOfGetDeviceResponse_WhenDeviceExists()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetDeviceByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Device);

        // Act
        var result = await _fixture.DevicesController.Get(_fixture.Id, _fixture.CancellationToken);
        var objectResult = result.Result.As<OkObjectResult>();
        var getCompanyResponse = objectResult.Value.As<GetDeviceResponse>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<GetDeviceResponse>>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        getCompanyResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_Should_ReturnActionResultOfGetDeviceResponse_WhenRequestIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<CreateDeviceCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Device);

        // Act
        var result = await _fixture.DevicesController.Create(_fixture.CreateDeviceRequest, _fixture.CancellationToken);
        var objectResult = result.Result.As<CreatedAtActionResult>();
        var getCompanyResponse = objectResult.Value.As<GetDeviceResponse>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<GetDeviceResponse>>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        getCompanyResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task Update_Should_ReturnNoContentResult_WhenDeviceExistsAndRequestIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetDeviceByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Device);

        // Act
        var result = await _fixture.DevicesController
            .Update(_fixture.Id, _fixture.UpdateDeviceRequest, _fixture.CancellationToken);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task Delete_Should_ReturnNoContentResult_WhenDeviceExists()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetDeviceByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Device);

        // Act
        var result = await _fixture.DevicesController.Delete(_fixture.Id, _fixture.CancellationToken);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
}
