using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Enums;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Fixtures
{
    public class CacheServiceFixture
    {
        public CacheServiceFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockDistributedCache = fixture.Freeze<Mock<IDistributedCache>>();
            MockLogger = fixture.Freeze<Mock<ILogger<CacheService>>>();

            MockCacheService = new(
                MockDistributedCache.Object,
                MockLogger.Object);

            Key = "key";
            Bytes = new byte[] { 123, 125 };
            User = GetUser();
        }

        public CacheService MockCacheService { get; }
        public Mock<IDistributedCache> MockDistributedCache { get; }
        public Mock<ILogger<CacheService>> MockLogger { get; }

        public byte[] Bytes { get; }
        public string Key { get; }
        public User User { get; }

        private User GetUser()
        {
            return new()
            {
                Id = 1,
                Username = Key,
                Email = Key,
                Role = UserRole.User,
                PasswordHash = Bytes,
                PasswordSalt = Bytes
            };
        }
    }
}
