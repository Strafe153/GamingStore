using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Users.Commands.Delete;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Users.Commands.Fixtures;

public class DeleteUserCommandHandlerFixture
{
    public DeleteUserCommandHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockRepository = fixture.Freeze<Mock<IUserRepository>>();
        MockUserService = fixture.Freeze<Mock<IUserService>>();
        MockPictureService = fixture.Freeze<Mock<IPictureService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<DeleteUserCommandHandler>>>();

        DeleteUserCommandHandler = new DeleteUserCommandHandler(
            MockRepository.Object,
            MockUserService.Object,
            MockPictureService.Object,
            MockLogger.Object);

        Name = "Name";
        SucceededResult = IdentityResult.Success;
        FailedResult = IdentityResult.Failed();
        User = GetUser();
        DeleteUserCommand = GetDeleteUserCommand();
    }
    public DeleteUserCommandHandler DeleteUserCommandHandler { get; }
    public Mock<IUserRepository> MockRepository { get; }
    public Mock<IUserService> MockUserService { get; }
    public Mock<IPictureService> MockPictureService { get; }
    public Mock<ILogger<DeleteUserCommandHandler>> MockLogger { get; }

    public string Name { get; }
    public CancellationToken CancellationToken { get; }
    public IdentityResult SucceededResult { get; }
    public IdentityResult FailedResult { get; }
    public User User { get; }
    public DeleteUserCommand DeleteUserCommand { get; }

    private User GetUser()
    {
        return new User(Name, Name, Name, Name, Name, Name);
    }

    private DeleteUserCommand GetDeleteUserCommand()
    {
        return new DeleteUserCommand(User);
    }
}
