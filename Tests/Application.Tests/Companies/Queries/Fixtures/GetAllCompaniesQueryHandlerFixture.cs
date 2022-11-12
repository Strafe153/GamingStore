using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Companies.Queries.GetAll;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Companies.Queries.Fixtures;

public class GetAllCompaniesQueryHandlerFixture
{
    public GetAllCompaniesQueryHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockRepository = fixture.Freeze<Mock<IRepository<Company>>>();
        MockCacheService = fixture.Freeze<Mock<ICacheService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<GetAllCompaniesQueryHandler>>>();

        GetAllCompaniesQueryHandler = new GetAllCompaniesQueryHandler(
            MockRepository.Object,
            MockCacheService.Object,
            MockLogger.Object);

        Name = "Name";
        GetAllCompaniesQuery = GetGetAllCompaniesQuery();
        PaginatedList = GetPaginatedList();
    }

    public GetAllCompaniesQueryHandler GetAllCompaniesQueryHandler { get; }
    public Mock<IRepository<Company>> MockRepository { get; }
    public Mock<ICacheService> MockCacheService { get; }
    public Mock<ILogger<GetAllCompaniesQueryHandler>> MockLogger { get; }

    public string Name { get; }
    public CancellationToken CancellationToken { get; }
    public GetAllCompaniesQuery GetAllCompaniesQuery { get; }
    public PaginatedList<Company> PaginatedList { get; }

    private GetAllCompaniesQuery GetGetAllCompaniesQuery()
    {
        return new GetAllCompaniesQuery(1, 5);
    }

    private Company GetCompany()
    {
        return new Company(Name, Name);
    }

    private List<Company> GetCompanies()
    {
        return new List<Company>()
        {
            GetCompany(),
            GetCompany()
        };
    }

    private PaginatedList<Company> GetPaginatedList()
    {
        return new PaginatedList<Company>(GetCompanies(), 6, 1, 5);
    }
}
