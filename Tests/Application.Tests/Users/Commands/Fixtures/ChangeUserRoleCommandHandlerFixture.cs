using Application.Abstractions.Repositories;
using Application.Users.Commands.ChangeRole;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Users.Commands.Fixtures;

public class ChangeUserRoleCommandHandlerFixture
{
    public ChangeUserRoleCommandHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockRepository = fixture.Freeze<Mock<IUserRepository>>();
        MockLogger = fixture.Freeze<Mock<ILogger<ChangeUserRoleCommandHandler>>>();

        ChangeUserRoleCommandHandler = new ChangeUserRoleCommandHandler(
            MockRepository.Object,
            MockLogger.Object);

        Name = "Name";
        SucceededResult = IdentityResult.Success;
        FailedResult = IdentityResult.Failed();
        User = GetUser();
        ChangeUserRoleCommand = GetChangeUserRoleCommand();
    }

    public ChangeUserRoleCommandHandler ChangeUserRoleCommandHandler { get; }
    public Mock<IUserRepository> MockRepository { get; }
    public Mock<ILogger<ChangeUserRoleCommandHandler>> MockLogger { get; }

    public string Name { get; }
    public CancellationToken CancellationToken { get; }
    public IdentityResult SucceededResult { get; }
    public IdentityResult FailedResult { get; }
    public User User { get; }
    public ChangeUserRoleCommand ChangeUserRoleCommand { get; }

    private User GetUser()
    {
        return new User(Name, Name, Name, Name, Name, Name);
    }

    private ChangeUserRoleCommand GetChangeUserRoleCommand()
    {
        return new ChangeUserRoleCommand()
        {
            User = User,
            Role = Name
        };
    }
}
