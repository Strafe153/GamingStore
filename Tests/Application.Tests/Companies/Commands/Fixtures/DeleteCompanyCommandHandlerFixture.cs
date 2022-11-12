using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Companies.Commands.Delete;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Companies.Commands.Fixtures;

public class DeleteCompanyCommandHandlerFixture
{
    public DeleteCompanyCommandHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockRepository = fixture.Freeze<Mock<IRepository<Company>>>();
        MockUnitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();
        MockPictureService = fixture.Freeze<Mock<IPictureService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<DeleteCompanyCommandHandler>>>();

        DeleteCompanyCommandHandler = new DeleteCompanyCommandHandler(
            MockRepository.Object,
            MockUnitOfWork.Object,
            MockPictureService.Object,
            MockLogger.Object);

        Name = "Name";
        Company = GetCompany();
        DeleteCompanyCommand = GetDeleteCompanyCommand();
    }
    public DeleteCompanyCommandHandler DeleteCompanyCommandHandler { get; }
    public Mock<IRepository<Company>> MockRepository { get; }
    public Mock<IUnitOfWork> MockUnitOfWork { get; }
    public Mock<IPictureService> MockPictureService { get; }
    public Mock<ILogger<DeleteCompanyCommandHandler>> MockLogger { get; }

    public string Name { get; }
    public CancellationToken CancellationToken { get; }
    public Company Company { get; }
    public DeleteCompanyCommand DeleteCompanyCommand { get; }

    private Company GetCompany()
    {
        return new Company(Name, Name);
    }

    private DeleteCompanyCommand GetDeleteCompanyCommand()
    {
        return new DeleteCompanyCommand(Company);
    }
}
