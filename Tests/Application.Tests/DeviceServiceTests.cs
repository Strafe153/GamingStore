﻿using Application.Tests.Fixtures;
using Core.Entities;
using Core.Models;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Application.Tests
{
    public class DeviceServiceTests : IClassFixture<DeviceServiceFixture>
    {
        private readonly DeviceServiceFixture _fixture;

        public DeviceServiceTests(DeviceServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllAsync_ValidParameters_ReturnsPaginatedListOfDevice()
        {
            // Arrange
            _fixture.MockDeviceRepository
                .Setup(r => r.GetAllAsync(
                    It.IsAny<int>(), 
                    It.IsAny<int>(), 
                    It.IsAny<Expression<Func<Device, bool>>>()))
                .ReturnsAsync(_fixture.PaginatedList);

            // Act
            var result = await _fixture.MockDeviceService.GetAllAsync(_fixture.Id, _fixture.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeOfType<PaginatedList<Device>>();
        }

        [Fact]
        public async Task GetByIdAsync_ExistingDevice_ReturnsDevice()
        {
            // Arrange
            _fixture.MockDeviceRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.Device);

            // Act
            var result = await _fixture.MockDeviceService.GetByIdAsync(_fixture.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Device>();
        }

        [Fact]
        public async Task GetByIdAsync_NonexistingDevice_ThrowsNullReferenceException()
        {
            // Arrange
            _fixture.MockDeviceRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Device)null!);

            // Act
            var result = async () => await _fixture.MockDeviceService.GetByIdAsync(_fixture.Id);

            // Assert
            result.Should().NotBeNull();
            await result.Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public async Task GetByCompanyAsync_ExistingCompany_ReturnsPaginatedListOfDevice()
        {
            // Arrange
            _fixture.MockDeviceRepository
                .Setup(r => r.GetAllAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Expression<Func<Device, bool>>>()))
                .ReturnsAsync(_fixture.PaginatedList);

            // Act
            var result = await _fixture.MockDeviceService.GetByCompanyAsync(_fixture.Id, _fixture.Id, _fixture.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeOfType<PaginatedList<Device>>();
        }

        [Fact]
        public void CreateAsync_ValidDevice_ReturnsTask()
        {
            // Act
            var result = _fixture.MockDeviceService.CreateAsync(_fixture.Device);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void UpdateAsync_ValidDevice_ReturnsTask()
        {
            // Act
            var result = _fixture.MockDeviceService.UpdateAsync(_fixture.Device);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void DeleteAsync_ValidDevice_ReturnsTask()
        {
            // Act
            var result = _fixture.MockDeviceService.DeleteAsync(_fixture.Device);

            // Assert
            result.Should().NotBeNull();
        }
    }
}