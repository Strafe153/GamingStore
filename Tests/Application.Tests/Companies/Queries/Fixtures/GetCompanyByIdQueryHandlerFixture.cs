using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Companies.Queries.GetById;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Companies.Queries.Fixtures;

public class GetCompanyByIdQueryHandlerFixture
{
	public GetCompanyByIdQueryHandlerFixture()
	{
		var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var companyFaker = new Faker<Company>()
            .CustomInstantiator(f => new(
                f.Company.CompanyName(),
                f.Internet.Url()));

        var getCompanyByIdQueryFaker = new Faker<GetCompanyByIdQuery>()
            .CustomInstantiator(f => new(f.Random.Int(1, 5000)));

        MockRepository = fixture.Freeze<Mock<IRepository<Company>>>();
        MockCacheService = fixture.Freeze<Mock<ICacheService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<GetCompanyByIdQueryHandler>>>();

        GetCompanyByIdQueryHandler = new GetCompanyByIdQueryHandler(
            MockRepository.Object,
            MockCacheService.Object,
            MockLogger.Object);

        Company = companyFaker.Generate();
        GetCompanyByIdQuery = getCompanyByIdQueryFaker.Generate();
    }

    public GetCompanyByIdQueryHandler GetCompanyByIdQueryHandler { get; }
    public Mock<IRepository<Company>> MockRepository { get; }
    public Mock<ICacheService> MockCacheService { get; }
    public Mock<ILogger<GetCompanyByIdQueryHandler>> MockLogger { get; }

    public CancellationToken CancellationToken { get; }
    public Company Company { get; }
    public GetCompanyByIdQuery GetCompanyByIdQuery { get; }
}
