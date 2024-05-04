using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Companies.Commands.Create;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Companies.Commands.Fixtures;

public class CreateCompanyCommandHandlerFixture
{
	public CreateCompanyCommandHandlerFixture()
	{
		var fixture = new Fixture().Customize(new AutoMoqCustomization());

		var createCompanyCommandFaker = new Faker<CreateCompanyCommand>()
			.RuleFor(c => c.Name, f => f.Company.CompanyName());

		MockCompanyRepository = fixture.Freeze<Mock<IRepository<Company>>>();
		MockDatabaseRepository = fixture.Freeze<Mock<IDatabaseRepository>>();
		MockPictureService = fixture.Freeze<Mock<IPictureService>>();
		MockLogger = fixture.Freeze<Mock<ILogger<CreateCompanyCommandHandler>>>();

		CreateCompanyCommandHandler = new CreateCompanyCommandHandler(
			MockCompanyRepository.Object,
			MockDatabaseRepository.Object,
			MockPictureService.Object,
			MockLogger.Object);

		CreateCompanyCommand = createCompanyCommandFaker.Generate();
	}

	public CreateCompanyCommandHandler CreateCompanyCommandHandler { get; }
	public Mock<IRepository<Company>> MockCompanyRepository { get; }
	public Mock<IDatabaseRepository> MockDatabaseRepository { get; }
	public Mock<IPictureService> MockPictureService { get; }
	public Mock<ILogger<CreateCompanyCommandHandler>> MockLogger { get; }

	public CancellationToken CancellationToken { get; }
	public CreateCompanyCommand CreateCompanyCommand { get; }
}
