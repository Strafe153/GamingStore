using Application.Companies.Commands.Create;
using Application.Companies.Commands.Update;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Bogus;
using Domain.Entities;
using Domain.Shared.PageParameters;
using Domain.Shared.Paging;
using MediatR;
using Moq;
using Presentation.AutoMapperProfiles;
using Presentation.Controllers.V1;

namespace Presentation.Tests.V1.Fixtures;

public class CompaniesControllerFixture
{
    public CompaniesControllerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var companyFaker = new Faker<Company>()
            .CustomInstantiator(f => new(
                f.Company.CompanyName(),
                f.Internet.Url()));

        var createCompanyRequestFaker = new Faker<CreateCompanyRequest>()
            .CustomInstantiator(f => new(
                f.Company.CompanyName(),
                null));

        var updateCompanyRequestFaker = new Faker<UpdateCompanyRequest>()
            .CustomInstantiator(f => new(
                f.Company.CompanyName(),
                null));

        var totalItemsCount = Random.Shared.Next(2, 50);

        var pagedListFaker = new Faker<PagedList<Company>>()
            .CustomInstantiator(f => new(
                companyFaker.Generate(totalItemsCount),
                totalItemsCount,
                f.Random.Int(1, 2),
                f.Random.Int(1, 2)))
            .RuleFor(l => l.PageSize, (f, l) => f.Random.Int(1, l.TotalItems))
            .RuleFor(l => l.CurrentPage, (f, l) => f.Random.Int(1, l.TotalPages));

        MockSender = fixture.Freeze<Mock<ISender>>();

        Mapper = new MapperConfiguration(options =>
        {
            options.AddProfile(new CompanyProfile());
        }).CreateMapper();

        CompaniesController = new CompaniesController(
            MockSender.Object,
            Mapper);

        Id = Random.Shared.Next(1, 5000);

        PageParameters = new()
        {
            PageNumber = Random.Shared.Next(1, 500),
            PageSize = Random.Shared.Next(1, 500)
        };

        Company = companyFaker.Generate();
        CreateCompanyRequest = createCompanyRequestFaker.Generate();
        UpdateCompanyRequest = updateCompanyRequestFaker.Generate();
        PagedList = pagedListFaker.Generate();
    }

    public CompaniesController CompaniesController { get; }
    public Mock<ISender> MockSender { get; }
    public IMapper Mapper { get; }

    public int Id { get; }
    public PageParameters PageParameters { get; set; }
    public CancellationToken CancellationToken { get; }
    public Company Company { get; }
    public CreateCompanyRequest CreateCompanyRequest { get; }
    public UpdateCompanyRequest UpdateCompanyRequest { get; }
    public PagedList<Company> PagedList { get; }
}
