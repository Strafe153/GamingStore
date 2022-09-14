using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Fixtures
{
    public class PictureServiceFixture
    {
        public PictureServiceFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockPictureRepository = fixture.Freeze<Mock<IPictureRepository>>();
            MockLogger = fixture.Freeze<Mock<ILogger<PictureService>>>();

            MockPictureService = new(
                MockPictureRepository.Object,
                MockLogger.Object);

            ValidPath = "../../../../../Application/Assets/Images/default_profile_pic.jpg";
            InvalidPath = null;
        }

        public PictureService MockPictureService { get; }
        public Mock<IPictureRepository> MockPictureRepository { get; }
        public Mock<ILogger<PictureService>> MockLogger { get; }

        public string ValidPath { get; }
        public string? InvalidPath { get; }
        public IFormFile? Picture { get; }
    }
}
