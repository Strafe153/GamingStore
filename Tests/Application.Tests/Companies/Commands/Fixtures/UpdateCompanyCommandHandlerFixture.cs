using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Companies.Commands.Update;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Companies.Commands.Fixtures;

public class UpdateCompanyCommandHandlerFixture
{
    public UpdateCompanyCommandHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockRepository = fixture.Freeze<Mock<IRepository<Company>>>();
        MockUnitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();
        MockPictureService = fixture.Freeze<Mock<IPictureService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<UpdateCompanyCommandHandler>>>();

        UpdateCompanyCommandHandler = new UpdateCompanyCommandHandler(
            MockRepository.Object,
            MockUnitOfWork.Object,
            MockPictureService.Object,
            MockLogger.Object);

        Name = "Name";
        Company = GetCompany();
        UpdateCompanyCommand = GetUpdateCompanyCommand();
    }

    public UpdateCompanyCommandHandler UpdateCompanyCommandHandler { get; }
    public Mock<IRepository<Company>> MockRepository { get; }
    public Mock<IUnitOfWork> MockUnitOfWork { get; }
    public Mock<IPictureService> MockPictureService { get; }
    public Mock<ILogger<UpdateCompanyCommandHandler>> MockLogger { get; }

    public string Name { get; }
    public IFormFile? Picture { get; }
    public CancellationToken CancellationToken { get; }
    public Company Company { get; }
    public UpdateCompanyCommand UpdateCompanyCommand { get; }

    private Company GetCompany()
    {
        return new Company(Name, Name);
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
}
