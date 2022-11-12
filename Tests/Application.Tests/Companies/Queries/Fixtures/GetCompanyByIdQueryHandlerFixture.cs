using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Companies.Queries.GetById;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Companies.Queries.Fixtures;

public class GetCompanyByIdQueryHandlerFixture
{
	public GetCompanyByIdQueryHandlerFixture()
	{
		var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockRepository = fixture.Freeze<Mock<IRepository<Company>>>();
        MockCacheService = fixture.Freeze<Mock<ICacheService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<GetCompanyByIdQueryHandler>>>();

        GetCompanyByIdQueryHandler = new GetCompanyByIdQueryHandler(
            MockRepository.Object,
            MockCacheService.Object,
            MockLogger.Object);

        Name = "Name";
        Company = GetCompany();
        GetCompanyByIdQuery = GetGetCompanyByIdQuery();
    }

    public GetCompanyByIdQueryHandler GetCompanyByIdQueryHandler { get; }
    public Mock<IRepository<Company>> MockRepository { get; }
    public Mock<ICacheService> MockCacheService { get; }
    public Mock<ILogger<GetCompanyByIdQueryHandler>> MockLogger { get; }

    public int Id { get; }
    public string Name { get; }
    public CancellationToken CancellationToken { get; }
    public Company Company { get; }
    public GetCompanyByIdQuery GetCompanyByIdQuery { get; }

    private GetCompanyByIdQuery GetGetCompanyByIdQuery()
    {
        return new GetCompanyByIdQuery(Id);
    }

    private Company GetCompany()
    {
        return new Company(Name, Name);
    }
}
