using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Users.Commands.ChangePassword;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Principal;

namespace Application.Tests.Users.Commands.Fixtures;

public class ChangeUserPasswordCommandHandlerFixture
{
    public ChangeUserPasswordCommandHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockRepository = fixture.Freeze<Mock<IUserRepository>>();
        MockUserService = fixture.Freeze<Mock<IUserService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<ChangeUserPasswordCommandHandler>>>();

        ChangeUserPasswordCommandHandler = new ChangeUserPasswordCommandHandler(
            MockRepository.Object,
            MockUserService.Object,
            MockLogger.Object);

        Name = "Name";
        SucceededResult = IdentityResult.Success;
        FailedResult = IdentityResult.Failed();
        User = GetUser();
        ChangeUserPasswordCommand = GetChangeUserPasswordCommand();
    }

    public ChangeUserPasswordCommandHandler ChangeUserPasswordCommandHandler { get; }
    public Mock<IUserRepository> MockRepository { get; }
    public Mock<IUserService> MockUserService { get; }
    public Mock<ILogger<ChangeUserPasswordCommandHandler>> MockLogger { get; }

    public string Name { get; }
    public CancellationToken CancellationToken { get; }
    public IdentityResult SucceededResult { get; }
    public IdentityResult FailedResult { get; }
    public User User { get; }
    public ChangeUserPasswordCommand ChangeUserPasswordCommand { get; }

    private User GetUser()
    {
        return new User(Name, Name, Name, Name, Name, Name);
    }

    private ChangeUserPasswordCommand GetChangeUserPasswordCommand()
    {
        return new ChangeUserPasswordCommand()
        {
            User = User,
            CurrentPassword = Name,
            NewPassword = Name
        };
    }
}
