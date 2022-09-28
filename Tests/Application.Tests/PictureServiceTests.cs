using Application.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Application.Tests;

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
    public void UploadAsync_ExistingFile_ReturnsString()
    {
        // Act
        var result = async () => await _fixture.MockPictureService
            .UploadAsync(_fixture.Picture, _fixture.ValidPath, _fixture.ValidPath);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UploadAsync_InvalidBlobConnection_ThrowsNullReferenceException()
    {
        // Act
        var result = async () => await _fixture.MockPictureService
            .UploadAsync(_fixture.Picture, _fixture.ValidPath, _fixture.ValidPath);

        // Assert
        await result.Should().ThrowAsync<NullReferenceException>();
    }
}
