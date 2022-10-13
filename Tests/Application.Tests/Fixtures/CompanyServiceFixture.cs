using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Fixtures;

public class CompanyServiceFixture
{
    public CompanyServiceFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockCompanyRepository = fixture.Freeze<Mock<IRepository<Company>>>();
        MockCacheService = fixture.Freeze<Mock<ICacheService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<CompanyService>>>();

        MockCompanyService = new CompanyService(
            MockCompanyRepository.Object,
            MockCacheService.Object,
            MockLogger.Object);

        Id = 1;
        Company = GetCompany();
        PaginatedList = new PaginatedList<Company>(GetCompanies(), 6, 1, 5);
        DbUpdateException = new DbUpdateException();
    }

    public CompanyService MockCompanyService { get; }
    public Mock<IRepository<Company>> MockCompanyRepository { get; }
    public Mock<ICacheService> MockCacheService { get; }
    public Mock<ILogger<CompanyService>> MockLogger { get; }

    public int Id { get; }
    public Company Company { get; }
    public PaginatedList<Company> PaginatedList { get; }
    public DbUpdateException DbUpdateException { get; }

    private Company GetCompany()
    {
        return new Company()
        {
            Id = Id,
            Name = "Name"
        };
    }

    private List<Company> GetCompanies()
    {
        return new List<Company>()
        {
            Company,
            Company
        };
    }
}
