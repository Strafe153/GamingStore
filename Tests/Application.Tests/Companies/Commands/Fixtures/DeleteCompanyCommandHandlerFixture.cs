using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Companies.Commands.Delete;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Companies.Commands.Fixtures;

public class DeleteCompanyCommandHandlerFixture
{
    public DeleteCompanyCommandHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var companyFaker = new Faker<Company>()
            .CustomInstantiator(f => new(
                f.Company.CompanyName(),
                f.Internet.Url()));

        var deleteCompanyCommandFaker = new Faker<DeleteCompanyCommand>()
            .CustomInstantiator(f => new(companyFaker.Generate()));

        MockCompanyRepository = fixture.Freeze<Mock<IRepository<Company>>>();
        MockDatabaseRepository = fixture.Freeze<Mock<IDatabaseRepository>>();
        MockPictureService = fixture.Freeze<Mock<IPictureService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<DeleteCompanyCommandHandler>>>();

        DeleteCompanyCommandHandler = new DeleteCompanyCommandHandler(
            MockCompanyRepository.Object,
            MockDatabaseRepository.Object,
            MockPictureService.Object,
            MockLogger.Object);

        DeleteCompanyCommand = deleteCompanyCommandFaker.Generate();
    }

    public DeleteCompanyCommandHandler DeleteCompanyCommandHandler { get; }
    public Mock<IRepository<Company>> MockCompanyRepository { get; }
    public Mock<IDatabaseRepository> MockDatabaseRepository { get; }
    public Mock<IPictureService> MockPictureService { get; }
    public Mock<ILogger<DeleteCompanyCommandHandler>> MockLogger { get; }

    public CancellationToken CancellationToken { get; }
    public DeleteCompanyCommand DeleteCompanyCommand { get; }
}
