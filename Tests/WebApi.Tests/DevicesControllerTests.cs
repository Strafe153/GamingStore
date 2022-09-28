using Core.Dtos;
using Core.Dtos.DeviceDtos;
using Core.Entities;
using Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Tests.Fixtures;
using Xunit;

namespace WebApi.Tests;

public class DevicesControllerTests : IClassFixture<DevicesControllerFixture>
{
    private readonly DevicesControllerFixture _fixture;

    public DevicesControllerTests(DevicesControllerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAsync_ValidPageParameters_ReturnsActionResultOfPageDtoOfDeviceReadDto()
    {
        // Arrange
        _fixture.MockDeviceService
            .Setup(s => s.GetAllAsync(
                It.IsAny<int>(), 
                It.IsAny<int>(), 
                It.IsAny<string>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.DevicePaginatedList);

        _fixture.MockMapper
            .Setup(m => m.Map<PageDto<DeviceReadDto>>(It.IsAny<PaginatedList<Device>>()))
            .Returns(_fixture.DevicePageDto);

        // Act
        var result = await _fixture.MockDevicesController.GetAsync(_fixture.PageParameters, _fixture.CancellationToken);
        var objectResult = result.Result.As<OkObjectResult>();
        var pageDto = objectResult.Value.As<PageDto<DeviceReadDto>>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<PageDto<DeviceReadDto>>>();
        objectResult.StatusCode.Should().Be(200);
        pageDto.Entities.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAsync_ExistingDevice_ReturnsActionResultOfDeviceReadDto()
    {
        // Arrange
        _fixture.MockDeviceService
            .Setup(s => s.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Device);

        _fixture.MockMapper
            .Setup(m => m.Map<DeviceReadDto>(It.IsAny<Device>()))
            .Returns(_fixture.DeviceReadDto);

        // Act
        var result = await _fixture.MockDevicesController.GetAsync(_fixture.Id, _fixture.CancellationToken);
        var objectResult = result.Result.As<OkObjectResult>();
        var readDto = objectResult.Value.As<DeviceReadDto>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<DeviceReadDto>>();
        objectResult.StatusCode.Should().Be(200);
        readDto.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsActionResultOfDeviceReadDto()
    {
        // Arrange
        _fixture.MockMapper
            .Setup(m => m.Map<Device>(It.IsAny<DeviceBaseDto>()))
            .Returns(_fixture.Device);

        _fixture.MockMapper
            .Setup(m => m.Map<DeviceReadDto>(It.IsAny<Device>()))
            .Returns(_fixture.DeviceReadDto);

        // Act
        var result = await _fixture.MockDevicesController.CreateAsync(_fixture.DeviceCreateUpdateDto);
        var objectResult = result.Result.As<CreatedAtActionResult>();
        var readDto = objectResult.Value.As<DeviceReadDto>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<DeviceReadDto>>();
        objectResult.StatusCode.Should().Be(201);
        readDto.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_ExistingDeviceValidDto_ReturnsNoContentResult()
    {
        // Arrange
        _fixture.MockDeviceService
            .Setup(s => s.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Device);

        // Act
        var result = await _fixture.MockDevicesController.UpdateAsync(_fixture.Id, _fixture.DeviceCreateUpdateDto);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task DeleteAsync_ExistingDevice_ReturnsNoContentResult()
    {
        // Arrange
        _fixture.MockDeviceService
            .Setup(s => s.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Device);

        // Act
        var result = await _fixture.MockDevicesController.DeleteAsync(_fixture.Id);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(204);
    }
}
