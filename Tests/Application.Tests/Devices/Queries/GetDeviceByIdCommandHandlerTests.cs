using Application.Tests.Devices.Queries.Fixtures;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Tests.Devices.Queries;

public class GetDeviceByIdCommandHandlerTests : IClassFixture<GetDeviceByIdCommandHandlerFixture>
{
    private readonly GetDeviceByIdCommandHandlerFixture _fixture;

    public GetDeviceByIdCommandHandlerTests(GetDeviceByIdCommandHandlerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Handle_Should_ReturnDeviceFromRepository_WhenDeviceExists()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<Device>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Device)null!);

        _fixture.MockRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Device);

        // Act
        var result = await _fixture.GetDeviceByIdQueryHandler.Handle(_fixture.GetDeviceByIdQuery, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<Device>();
    }

    [Fact]
    public async Task Handle_Should_ReturnDeviceFromCache_WhenDeviceExists()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<Device>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Device);

        // Act
        var result = await _fixture.GetDeviceByIdQueryHandler.Handle(_fixture.GetDeviceByIdQuery, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<Device>();
    }

    [Fact]
    public async Task Handle_Should_ThrowNullReferenceException_WhenDeviceDoesNotExist()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<Device>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Device)null!);

        _fixture.MockRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Device)null!);

        // Act
        var result = async () => await _fixture.GetDeviceByIdQueryHandler
            .Handle(_fixture.GetDeviceByIdQuery, _fixture.CancellationToken);

        // Assert
        await result.Should().ThrowAsync<NullReferenceException>();
    }
}
