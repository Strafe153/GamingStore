using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Companies.Commands.Update;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Companies.Commands.Fixtures;

public class UpdateCompanyCommandHandlerFixture
{
	public UpdateCompanyCommandHandlerFixture()
	{
		var fixture = new Fixture().Customize(new AutoMoqCustomization());

		var companyFaker = new Faker<Company>()
			.CustomInstantiator(f => new(
				f.Company.CompanyName(),
				f.Internet.Url()));

		var updateCompanyCommandFaker = new Faker<UpdateCompanyCommand>()
			.RuleFor(c => c.Name, f => f.Company.CompanyName())
			.RuleFor(c => c.Company, companyFaker);

		MockCompanyRepository = fixture.Freeze<Mock<IRepository<Company>>>();
		MockDatabaseRepository = fixture.Freeze<Mock<IDatabaseRepository>>();
		MockPictureService = fixture.Freeze<Mock<IPictureService>>();
		MockLogger = fixture.Freeze<Mock<ILogger<UpdateCompanyCommandHandler>>>();

		UpdateCompanyCommandHandler = new UpdateCompanyCommandHandler(
			MockCompanyRepository.Object,
			MockDatabaseRepository.Object,
			MockPictureService.Object,
			MockLogger.Object);

		UpdateCompanyCommand = updateCompanyCommandFaker.Generate();
	}

	public UpdateCompanyCommandHandler UpdateCompanyCommandHandler { get; }
	public Mock<IRepository<Company>> MockCompanyRepository { get; }
	public Mock<IDatabaseRepository> MockDatabaseRepository { get; }
	public Mock<IPictureService> MockPictureService { get; }
	public Mock<ILogger<UpdateCompanyCommandHandler>> MockLogger { get; }

	public CancellationToken CancellationToken { get; }
	public UpdateCompanyCommand UpdateCompanyCommand { get; }
}
