using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Companies.Queries.GetAll;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Domain.Entities;
using Domain.Shared.Paging;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Companies.Queries.Fixtures;

public class GetAllCompaniesQueryHandlerFixture
{
	public GetAllCompaniesQueryHandlerFixture()
	{
		var fixture = new Fixture().Customize(new AutoMoqCustomization());

		var getAllCompaniesQueryFaker = new Faker<GetAllCompaniesQuery>()
			.CustomInstantiator(f => new(
				f.Random.Int(1, 500),
				f.Random.Int(1, 500)));

		var companyFaker = new Faker<Company>()
			.CustomInstantiator(f => new(
				f.Company.CompanyName(),
				f.Internet.Url()));

		var totalItemsCount = Random.Shared.Next(2, 50);

		var pagedListFaker = new Faker<PagedList<Company>>()
			.CustomInstantiator(f => new(
				companyFaker.Generate(totalItemsCount),
				totalItemsCount,
				f.Random.Int(1, 2),
				f.Random.Int(1, 2)))
			.RuleFor(l => l.PageSize, (f, l) => f.Random.Int(1, l.TotalItems))
			.RuleFor(l => l.CurrentPage, (f, l) => f.Random.Int(1, l.TotalPages));

		MockRepository = fixture.Freeze<Mock<IRepository<Company>>>();
		MockCacheService = fixture.Freeze<Mock<ICacheService>>();
		MockLogger = fixture.Freeze<Mock<ILogger<GetAllCompaniesQueryHandler>>>();

		GetAllCompaniesQueryHandler = new GetAllCompaniesQueryHandler(
			MockRepository.Object,
			MockCacheService.Object,
			MockLogger.Object);

		GetAllCompaniesQuery = getAllCompaniesQueryFaker.Generate();
		PagedList = pagedListFaker.Generate();
	}

	public GetAllCompaniesQueryHandler GetAllCompaniesQueryHandler { get; }
	public Mock<IRepository<Company>> MockRepository { get; }
	public Mock<ICacheService> MockCacheService { get; }
	public Mock<ILogger<GetAllCompaniesQueryHandler>> MockLogger { get; }

	public CancellationToken CancellationToken { get; }
	public GetAllCompaniesQuery GetAllCompaniesQuery { get; }
	public PagedList<Company> PagedList { get; }
}
