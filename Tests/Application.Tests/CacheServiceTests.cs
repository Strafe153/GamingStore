using Application.Tests.Fixtures;
using Core.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Tests
{
    public class CacheServiceTests : IClassFixture<CacheServiceFixture>
    {
        private readonly CacheServiceFixture _fixture;

        public CacheServiceTests(CacheServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAsync_ExistingData_ReturnsData()
        {
            // Arrange
            _fixture.MockDistributedCache
                .Setup(d => d.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_fixture.Bytes);

            // Act
            var result = await _fixture.MockCacheService.GetAsync<User>(_fixture.Key);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAsync_NonexistingData_ReturnsTask()
        {
            // Arrange
            _fixture.MockDistributedCache
                .Setup(d => d.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[])null!);

            // Act
            var result = await _fixture.MockCacheService.GetAsync<User>(_fixture.Key);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void SetAsync_ValidData_ReturnsData()
        {
            // Act
            var result = async () => await _fixture.MockCacheService.SetAsync<User>(_fixture.Key, _fixture.User);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
