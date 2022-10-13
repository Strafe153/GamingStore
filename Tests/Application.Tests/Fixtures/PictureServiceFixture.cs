using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Fixtures;

public class PictureServiceFixture
{
    public PictureServiceFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockConfiguration = fixture.Freeze<Mock<IConfiguration>>();
        MockLogger = fixture.Freeze<Mock<ILogger<PictureService>>>();
        MockBlobServiceClient = fixture.Freeze<Mock<BlobServiceClient>>();

        MockPictureService = new PictureService(
            MockConfiguration.Object,
            MockLogger.Object,
            MockBlobServiceClient.Object);

        ValidPath = "../../../../../Application/Assets/Images/default_profile_pic.jpg";
        InvalidPath = null;
        Picture = GetFormFile().Result;
    }

    public PictureService MockPictureService { get; }
    public Mock<IConfiguration> MockConfiguration { get; }
    public Mock<ILogger<PictureService>> MockLogger { get; }
    public Mock<BlobServiceClient> MockBlobServiceClient { get; }

    public string ValidPath { get; }
    public string? InvalidPath { get; }
    public IFormFile? Picture { get; }

    private async Task<IFormFile> GetFormFile()
    {
        byte[] pictureAsBytes = await File.ReadAllBytesAsync(ValidPath);

        return new FormFile(new MemoryStream(pictureAsBytes), 0, pictureAsBytes.Length, "Picture", "picture.jpg")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpeg"
        };
    }
}
