using Microsoft.AspNetCore.Mvc;
using GamingStore.Models;
using GamingStore.Controllers;
using GamingStore.Dtos.Device;
using GamingStore.Repositories.Interfaces;
using Moq;
using Xunit;
using AutoMapper;

namespace GamingStore.Tests
{
    public class DevicesControllerTests
    {
        private static readonly Mock<IControllable<Device>> _devicesRepo = new();
        private static readonly Mock<ICompanyControllable> _companiesRepo = new();
        private static readonly Mock<IMapper> _mapper = new();
        private static readonly DevicesController _controller = new(
            _devicesRepo.Object, _companiesRepo.Object, _mapper.Object);

        [Fact]
        public async Task GetAllDevicesAsync_ValidData_ReturnsOkObjectResult()
        {
            // Act
            var result = await _controller.GetAllDevicesAsync();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<DeviceReadDto>>>(result);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetDevicesByCompanyAsync_ExistingCompany_ReturnsOkObjectResult()
        {
            // Arrange
            _companiesRepo.Setup(c => c.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Company());

            // Act
            var result = await _controller.GetDevicesByCompanyAsync(Guid.Empty);

            // Assert
            Assert.IsType<ActionResult<IEnumerable<DeviceReadDto>>>(result);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetDevicesByCompanyAsync_NonexistingCompany_ReturnsNotFoundObjectResult()
        {
            // Arrange
            _companiesRepo.Setup(c => c.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Company?)null);

            // Act
            var result = await _controller.GetDevicesByCompanyAsync(Guid.Empty);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetDeviceAsync_ExistingDevice_ReturnsOkObjectResult()
        {
            // Arrange
            _devicesRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Device());

            // Act
            var result = await _controller.GetDeviceAsync(Guid.Empty);

            // Assert
            Assert.IsType<ActionResult<DeviceReadDto>>(result);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetDeviceAsync_NonexistingDevice_ReturnsNotFoundObjectResult()
        {
            _devicesRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Device?)null);

            // Act
            var result = await _controller.GetDeviceAsync(Guid.Empty);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateDeviceAsync_ValidData_ReturnsCreatedAtActionResult()
        {
            // Arrange
            _mapper.Setup(m => m.Map<DeviceReadDto>(It.IsAny<Company>())).Returns(new DeviceReadDto());

            // Act
            var result = await _controller.CreateDeviceAsync(new DeviceCreateDto());

            // Assert
            Assert.IsType<ActionResult<DeviceReadDto>>(result);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task UpdateDeviceAsync_ExistingDevice_ReturnsNoContentResult()
        {
            // Arrange
            _devicesRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Device());

            // Act
            var result = await _controller.UpdateDeviceAsync(Guid.Empty, new DeviceUpdateDto());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateDeviceAsync_NonexistingDevice_ReturnsNotFoundObjectResult()
        {
            // Arrange
            _devicesRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Device?)null);

            // Act
            var result = await _controller.UpdateDeviceAsync(Guid.Empty, new DeviceUpdateDto());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteDeviceAsync_ExistingDevice_ReturnsNoContentResult()
        {
            // Arrange
            _devicesRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Device());

            // Act
            var result = await _controller.DeleteDeviceAsync(Guid.Empty);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteDeviceAsync_NonexistingDevice_ReturnsNotFoundObjectResult()
        {

            // Arrange
            _devicesRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Device?)null);

            // Act
            var result = await _controller.DeleteDeviceAsync(Guid.Empty);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
