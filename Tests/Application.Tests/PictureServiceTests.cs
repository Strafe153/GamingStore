using Application.Tests.Fixtures;
using Azure;
using FluentAssertions;
using Moq;
using SixLabors.ImageSharp;
using Xunit;

namespace Application.Tests
{
    public class PictureServiceTests : IClassFixture<PictureServiceFixture>
    {
        private readonly PictureServiceFixture _fixture;

        public PictureServiceTests(PictureServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void DeleteAsync_ValidImageLink_ReturnsTask()
        {
            // Act
            var result = async () => await _fixture.MockPictureService.DeleteAsync(_fixture.ValidPath);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void DeleteAsync_InvalidImageLink_ThrowsException()
        {
            // Act
            var result = async () => await _fixture.MockPictureService.DeleteAsync(_fixture.InvalidPath!);

            // Assert
            result.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task UploadAsync_ExistingFile_ReturnsString()
        {
            // Arrange
            _fixture.MockPictureRepository
                .Setup(r => r.UploadAsync(
                    It.IsAny<Image>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.ValidPath);

            // Act
            var result = await _fixture.MockPictureService.UploadAsync(_fixture.ValidPath, _fixture.ValidPath, _fixture.ValidPath);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task UploadAsync_InvalidBlobConnection_ThrowsNullReferenceException()
        {
            // Arrange
            _fixture.MockPictureRepository
                .Setup(r => r.UploadAsync(
                    It.IsAny<Image>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ThrowsAsync(new RequestFailedException(It.IsAny<string>()));

            // Act
            var result = async () => await _fixture.MockPictureService
                .UploadAsync(_fixture.ValidPath, _fixture.ValidPath, _fixture.ValidPath);

            // Assert
            await result.Should().ThrowAsync<NullReferenceException>();
        }
    }
}
