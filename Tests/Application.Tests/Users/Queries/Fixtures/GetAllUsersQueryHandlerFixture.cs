using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Users.Queries.GetAll;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Domain.Entities;
using Domain.Shared.Paging;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Users.Queries.Fixtures;

public class GetAllUsersQueryHandlerFixture
{
	public GetAllUsersQueryHandlerFixture()
	{
		var fixture = new Fixture().Customize(new AutoMoqCustomization());

		var getAllUsersQueryFaker = new Faker<GetAllUsersQuery>()
			.CustomInstantiator(f => new(
				f.Random.Int(1, 500),
				f.Random.Int(1, 500)));

		var userFaker = new Faker<User>()
			.CustomInstantiator(f => new(
				f.Name.FirstName(),
				f.Name.LastName(),
				f.Internet.Email(),
				f.Internet.UserName(),
				f.Phone.PhoneNumber(),
				null));

		var totalItemsCount = Random.Shared.Next(2, 50);

		var pagedListFaker = new Faker<PagedList<User>>()
			.CustomInstantiator(f => new(
				userFaker.Generate(totalItemsCount),
				totalItemsCount,
				f.Random.Int(1, 2),
				f.Random.Int(1, 2)))
			.RuleFor(l => l.PageSize, (f, l) => f.Random.Int(1, l.TotalItems))
			.RuleFor(l => l.CurrentPage, (f, l) => f.Random.Int(1, l.TotalPages));

		MockRepository = fixture.Freeze<Mock<IUserRepository>>();
		MockCacheService = fixture.Freeze<Mock<ICacheService>>();
		MockLogger = fixture.Freeze<Mock<ILogger<GetAllUsersQueryHandler>>>();

		GetAllUsersQueryHandler = new GetAllUsersQueryHandler(
			MockRepository.Object,
			MockCacheService.Object,
			MockLogger.Object);

		GetAllUsersQuery = getAllUsersQueryFaker.Generate();
		PagedList = pagedListFaker.Generate();
	}

	public GetAllUsersQueryHandler GetAllUsersQueryHandler { get; }
	public Mock<IUserRepository> MockRepository { get; }
	public Mock<ICacheService> MockCacheService { get; }
	public Mock<ILogger<GetAllUsersQueryHandler>> MockLogger { get; }

	public CancellationToken CancellationToken { get; }
	public GetAllUsersQuery GetAllUsersQuery { get; }
	public PagedList<User> PagedList { get; }
}
