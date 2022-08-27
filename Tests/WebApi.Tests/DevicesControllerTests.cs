using Core.Entities;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.CompanyViewModels;
using Core.ViewModels.DeviceViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Tests.Fixtures;
using Xunit;

namespace WebApi.Tests
{
    public class DevicesControllerTests : IClassFixture<DevicesControllerFixture>
    {
        private readonly DevicesControllerFixture _fixture;

        public DevicesControllerTests(DevicesControllerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAsync_ValidPageParameters_ReturnsActionResultOfPageViewModelOfDeviceReadViewModel()
        {
            // Arrange
            _fixture.MockDeviceService
                .Setup(s => s.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(_fixture.DevicePaginatedList);

            _fixture.MockDevicePaginatedMapper
                .Setup(m => m.Map(It.IsAny<PaginatedList<Device>>()))
                .Returns(_fixture.DevicePageViewModel);

            // Act
            var result = await _fixture.MockDevicesController.GetAsync(_fixture.PageParameters);
            var pageViewModel = result.Result.As<OkObjectResult>().Value.As<PageViewModel<DeviceReadViewModel>>();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<PageViewModel<DeviceReadViewModel>>>();
            pageViewModel.Entities.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetAsync_ExistingDevice_ReturnsActionResultOfDeviceReadViewModel()
        {
            // Arrange
            _fixture.MockDeviceService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.Device);

            _fixture.MockDeviceReadMapper
                .Setup(m => m.Map(It.IsAny<Device>()))
                .Returns(_fixture.DeviceReadViewModel);

            // Act
            var result = await _fixture.MockDevicesController.GetAsync(_fixture.Id);
            var readViewModel = result.Result.As<OkObjectResult>().Value.As<DeviceReadViewModel>();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<DeviceReadViewModel>>();
            readViewModel.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateAsync_ValidViewModel_ReturnsActionResultOfDeviceReadViewModel()
        {
            // Arrange
            _fixture.MockDeviceCreateMapper
                .Setup(m => m.Map(It.IsAny<DeviceBaseViewModel>()))
                .Returns(_fixture.Device);

            _fixture.MockDeviceReadMapper
                .Setup(m => m.Map(It.IsAny<Device>()))
                .Returns(_fixture.DeviceReadViewModel);

            // Act
            var result = await _fixture.MockDevicesController.CreateAsync(_fixture.DeviceBaseViewModel);
            var readViewModel = result.Result.As<CreatedAtActionResult>().Value.As<DeviceReadViewModel>();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<DeviceReadViewModel>>();
            readViewModel.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateAsync_ExistingDeviceValidViewModel_ReturnsNoContentResult()
        {
            // Arrange
            _fixture.MockDeviceService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.Device);

            // Act
            var result = await _fixture.MockDevicesController.UpdateAsync(_fixture.Id, _fixture.DeviceBaseViewModel);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteAsync_ExistingDevice_ReturnsNoContentResult()
        {
            // Arrange
            _fixture.MockDeviceService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.Device);

            // Act
            var result = await _fixture.MockDevicesController.DeleteAsync(_fixture.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
