using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Companies.Commands.Create;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Companies.Commands.Fixtures;

public class CreateCompanyCommandHandlerFixture
{
    public CreateCompanyCommandHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockRepository = fixture.Freeze<Mock<IRepository<Company>>>();
        MockUnitOfWork = fixture.Freeze<Mock<IUnitOfWork>>();
        MockPictureService = fixture.Freeze<Mock<IPictureService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<CreateCompanyCommandHandler>>>();

        CreateCompanyCommandHandler = new CreateCompanyCommandHandler(
            MockRepository.Object,
            MockUnitOfWork.Object,
            MockPictureService.Object,
            MockLogger.Object);

        Name = "Name";
        CreateCompanyCommand = GetCreateCompanyCommand();
    }

    public CreateCompanyCommandHandler CreateCompanyCommandHandler { get; }
    public Mock<IRepository<Company>> MockRepository { get; }
    public Mock<IUnitOfWork> MockUnitOfWork { get; }
    public Mock<IPictureService> MockPictureService { get; }
    public Mock<ILogger<CreateCompanyCommandHandler>> MockLogger { get; }

    public string Name { get; }
    public IFormFile? Picture { get; }
    public CancellationToken CancellationToken { get; }
    public CreateCompanyCommand CreateCompanyCommand { get; }

    private CreateCompanyCommand GetCreateCompanyCommand()
    {
        return new CreateCompanyCommand()
        {
            Name = Name,
            Picture = Picture
        };
    }
}
