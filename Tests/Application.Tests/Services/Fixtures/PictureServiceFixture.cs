using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Services.Fixtures;

public class PictureServiceFixture
{
    public PictureServiceFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockConfiguration = fixture.Freeze<Mock<IConfiguration>>();
        MockLogger = fixture.Freeze<Mock<ILogger<PictureService>>>();
        MockBlobServiceClient = fixture.Freeze<Mock<BlobServiceClient>>();
        MockBlobContainerClient = fixture.Freeze<Mock<BlobContainerClient>>();
        MockResponse = fixture.Freeze<Mock<Response<BlobContentInfo>>>();

        MockBlobServiceClient
            .Setup(c => c.Uri)
            .Returns(new Uri("https://127.0.0.1:10000/devstoreaccount1"));

        MockBlobServiceClient
            .Setup(c => c.GetBlobContainerClient(It.IsAny<string>()))
            .Returns(MockBlobContainerClient.Object);

        PictureService = new PictureService(
            MockConfiguration.Object,
            MockLogger.Object,
            MockBlobServiceClient.Object);

        ValidPath = "../../../../../Application/Common/Images/default_picture.jpg";
        InvalidPath = null;
        Picture = GetFormFile().Result;
    }

    public PictureService PictureService { get; }
    public Mock<IConfiguration> MockConfiguration { get; }
    public Mock<ILogger<PictureService>> MockLogger { get; }
    public Mock<BlobServiceClient> MockBlobServiceClient { get; }
    public Mock<BlobContainerClient> MockBlobContainerClient { get; }
    public Mock<Response<BlobContentInfo>> MockResponse { get; }

    public string ValidPath { get; }
    public string? InvalidPath { get; }
    public IFormFile? Picture { get; }

    private async Task<IFormFile> GetFormFile()
    {
        var pictureAsBytes = await File.ReadAllBytesAsync(ValidPath);

        return new FormFile(new MemoryStream(pictureAsBytes), 0, pictureAsBytes.Length, "ProfilePicture", "picture.jpg")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpeg"
        };
    }
}
