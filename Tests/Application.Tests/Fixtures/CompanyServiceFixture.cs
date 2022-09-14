using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Fixtures
{
    public class CompanyServiceFixture
    {
        public CompanyServiceFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockCompanyRepository = fixture.Freeze<Mock<IRepository<Company>>>();
            MockLogger = fixture.Freeze<Mock<ILogger<CompanyService>>>();

            MockCompanyService = new(
                MockCompanyRepository.Object,
                MockLogger.Object);

            Id = 1;
            Company = GetCompany();
            PaginatedList = GetPaginatedList();
        }

        public CompanyService MockCompanyService { get; }
        public Mock<IRepository<Company>> MockCompanyRepository { get; }
        public Mock<ILogger<CompanyService>> MockLogger { get; }

        public int Id { get; }
        public Company Company { get; }
        public PaginatedList<Company> PaginatedList { get; }

        private Company GetCompany()
        {
            return new()
            {
                Id = Id,
                Name = "Name"
            };
        }

        private List<Company> GetCompanies()
        {
            return new()
            {
                Company,
                Company
            };
        }

        private PaginatedList<Company> GetPaginatedList()
        {
            return new(GetCompanies(), 6, 1, 5);
        }
    }
}
