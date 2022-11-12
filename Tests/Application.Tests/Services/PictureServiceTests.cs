using Application.Tests.Services.Fixtures;
using Azure;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Tests.Services;

public class PictureServiceTests : IClassFixture<PictureServiceFixture>
{
    private readonly PictureServiceFixture _fixture;

    public PictureServiceTests(PictureServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void DeleteAsync_Should_ReturnTask_WhenImageLinkIsValid()
    {
        // Act
        var result = async () => await _fixture.PictureService.DeleteAsync(_fixture.ValidPath);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void DeleteASync_Should_ThrowRequestFailedException_WhenImageLinkIsInvalid()
    {
        // Act
        var result = async () => await _fixture.PictureService.DeleteAsync(_fixture.InvalidPath!);

        // Assert
        result.Should().ThrowAsync<RequestFailedException>();
    }

    [Fact]
    public async Task UploadAsync_Should_ReturnString_WhenFileExists()
    {
        // Arrange
        _fixture.MockBlobContainerClient
            .Setup(c => c.UploadBlobAsync(
                It.IsAny<string>(),
                It.IsAny<Stream>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.MockResponse.Object);

        // Act
        var result = await _fixture.PictureService.UploadAsync(_fixture.Picture, _fixture.ValidPath, Guid.NewGuid().ToString());

        // Assert
        result.Should().NotBeNull().And.BeOfType<string>();
    }

    [Fact]
    public async Task UploadAsync_Should_ThrowNullReferenceException_WhenBlobConnectionIsInvalid()
    {
        // Act
        var result = async () => await _fixture.PictureService
            .UploadAsync(_fixture.Picture, _fixture.ValidPath, _fixture.ValidPath);

        // Assert
        await result.Should().ThrowAsync<NullReferenceException>();
    }
}
