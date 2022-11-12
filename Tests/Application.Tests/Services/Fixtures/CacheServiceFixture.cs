using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Services.Fixtures;

public class CacheServiceFixture
{
    public CacheServiceFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockDistributedCache = fixture.Freeze<Mock<IDistributedCache>>();
        MockLogger = fixture.Freeze<Mock<ILogger<CacheService>>>();

        CacheService = new CacheService(
            MockDistributedCache.Object,
            MockLogger.Object);

        Key = "key";
        Bytes = new byte[] { 123, 125 };
        User = GetUser();
    }

    public CacheService CacheService { get; }
    public Mock<IDistributedCache> MockDistributedCache { get; }
    public Mock<ILogger<CacheService>> MockLogger { get; }

    public byte[] Bytes { get; }
    public string Key { get; }
    public User User { get; }

    private User GetUser()
    {
        return new User()
        {
            Id = 1,
            UserName = Key,
            Email = Key,
            PasswordHash = Key
        };
    }
}
