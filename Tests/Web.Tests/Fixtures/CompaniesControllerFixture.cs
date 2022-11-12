using Application.Companies.Commands.Create;
using Application.Companies.Commands.Update;
using Application.Companies.Queries;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Presentation.Controllers;

namespace Web.Tests.Fixtures;

public class CompaniesControllerFixture
{
	public CompaniesControllerFixture()
	{
		var fixture = new Fixture().Customize(new AutoMoqCustomization());

		MockSender = fixture.Freeze<Mock<ISender>>();
		MockMapper = fixture.Freeze<Mock<IMapper>>();

		CompaniesController = new CompaniesController(
			MockSender.Object,
			MockMapper.Object);

		Id = 1;
		Name = "Name";
		PageParameters = GetPageParameters();
		Company = GetCompany();
		GetCompanyResponse = GetGetCompanyResponse();
		CreateCompanyRequest = GetCreateCompanyRequest();
		CreateCompanyCommand = GetCreateCompanyCommand();
		UpdateCompanyRequest = GetUpdateCompanyRequest();
		UpdateCompanyCommand = GetUpdateCompanyCommand();
		PaginatedList = GetPaginatedList();
		PaginatedModel = GetPaginatedModel();
	}

	public CompaniesController CompaniesController { get; }
	public Mock<ISender> MockSender { get; }
	public Mock<IMapper> MockMapper { get; }

	public int Id { get; }
	public string Name { get; }
	public IFormFile? Picture { get; }
	public Unit Unit { get; }
	public PageParameters PageParameters { get; set; }
	public CancellationToken CancellationToken { get; }
	public Company Company { get; }
	public GetCompanyResponse GetCompanyResponse { get; }
	public CreateCompanyRequest CreateCompanyRequest { get; }
	public CreateCompanyCommand CreateCompanyCommand { get; }
	public UpdateCompanyRequest UpdateCompanyRequest { get; }
	public UpdateCompanyCommand UpdateCompanyCommand { get; }
	public PaginatedList<Company> PaginatedList { get; }
	public PaginatedModel<GetCompanyResponse> PaginatedModel { get; }

	private PageParameters GetPageParameters()
	{
		return new PageParameters()
		{
			PageNumber = 1,
			PageSize = 5
		};
	}

	private Company GetCompany()
	{
		return new Company(Name, Name);
	}

	private List<Company> GetCompanies()
	{
		return new List<Company>()
		{
			Company,
            Company
        };
	}

	private Device GetDevice()
	{
		return new Device(Name, DeviceCategory.Mouse, 24.99M, 500, Name, Id);
	}

	private List<Device> GetDevices()
	{
		return new List<Device>()
		{
			GetDevice(),
			GetDevice()
		};
	}

	private GetCompanyResponse GetGetCompanyResponse()
	{
		return new GetCompanyResponse(Id, Name, Name, GetDevices());
	}

	private List<GetCompanyResponse> GetCompanyResponses()
	{
		return new List<GetCompanyResponse>()
		{
			GetCompanyResponse,
			GetCompanyResponse
		};
	}

	private CreateCompanyRequest GetCreateCompanyRequest()
	{
		return new CreateCompanyRequest(Name, Picture);
	}

	private CreateCompanyCommand GetCreateCompanyCommand()
	{
		return new CreateCompanyCommand()
		{
			Name = Name,
			Picture = Picture
		};
	}

	private UpdateCompanyRequest GetUpdateCompanyRequest()
	{
		return new UpdateCompanyRequest(Name, Picture);
    }

    private UpdateCompanyCommand GetUpdateCompanyCommand()
    {
        return new UpdateCompanyCommand()
		{
			Company = Company,
			Name = Name,
			Picture = Picture
		};
    }

    private PaginatedList<Company> GetPaginatedList()
    {
        return new PaginatedList<Company>(GetCompanies(), 5, 1, 5);
    }

    private PaginatedModel<GetCompanyResponse> GetPaginatedModel()
	{
		return new PaginatedModel<GetCompanyResponse>()
		{
            CurrentPage = 1,
            TotalPages = 2,
            PageSize = 5,
            TotalItems = 6,
            HasPrevious = false,
            HasNext = true,
            Entities = GetCompanyResponses()
        };
	}
}
