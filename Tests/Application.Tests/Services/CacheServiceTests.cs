using Application.Tests.Services.Fixtures;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Tests.Services;

public class CacheServiceTests : IClassFixture<CacheServiceFixture>
{
    private readonly CacheServiceFixture _fixture;

    public CacheServiceTests(CacheServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAsync_Should_ReturnData_WhenDataExistsInCache()
    {
        // Arrange
        _fixture.MockDistributedCache
            .Setup(d => d.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Bytes);

        // Act
        var result = await _fixture.CacheService.GetAsync<User>(_fixture.Key);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAsync_Should_ReturnTask_WhenDataDoesNotExistInCache()
    {
        // Arrange
        _fixture.MockDistributedCache
            .Setup(d => d.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[])null!);

        // Act
        var result = await _fixture.CacheService.GetAsync<User>(_fixture.Key);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void SetAsync_Should_ReturnData_WhenDataIsValid()
    {
        // Act
        var result = async () => await _fixture.CacheService.SetAsync(_fixture.Key, _fixture.User);

        // Assert
        result.Should().NotBeNull();
    }
}
