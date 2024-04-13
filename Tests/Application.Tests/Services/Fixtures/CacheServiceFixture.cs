using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Domain.Entities;
using Domain.Shared;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Application.Tests.Services.Fixtures;

public class CacheServiceFixture
{
	public CacheServiceFixture()
	{
		var fixture = new Fixture().Customize(new AutoMoqCustomization());

		var userFaker = new Faker<User>()
			.CustomInstantiator(f => new(
				f.Name.FirstName(),
				f.Name.LastName(),
				f.Internet.Email(),
				f.Internet.UserName(),
				f.Phone.PhoneNumber(),
				null));

		MockDistributedCache = fixture.Freeze<Mock<IDistributedCache>>();
		MockOptions = fixture.Freeze<Mock<IOptions<CacheOptions>>>();
		MockLogger = fixture.Freeze<Mock<ILogger<CacheService>>>();

		CacheService = new CacheService(
			MockDistributedCache.Object,
			MockOptions.Object,
			MockLogger.Object);

		Key = new Faker().Internet.DomainName();
		Bytes = new byte[] { 123, 125 };
		User = userFaker.Generate();
	}

	public CacheService CacheService { get; }
	public Mock<IDistributedCache> MockDistributedCache { get; }
	public Mock<IOptions<CacheOptions>> MockOptions { get; }
	public Mock<ILogger<CacheService>> MockLogger { get; }

	public byte[] Bytes { get; }
	public string Key { get; }
	public CancellationToken CancellationToken { get; }
	public User User { get; }
}
