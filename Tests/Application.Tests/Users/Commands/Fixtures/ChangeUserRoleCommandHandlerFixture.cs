using Application.Abstractions.Repositories;
using Application.Users.Commands.ChangeRole;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
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

        var userFaker = new Faker<User>()
            .CustomInstantiator(f => new(
                f.Name.FirstName(),
                f.Name.LastName(),
                f.Internet.Email(),
                f.Internet.UserName(),
                f.Phone.PhoneNumber(),
                null));

        var changeUserRoleCommandFaker = new Faker<ChangeUserRoleCommand>()
            .RuleFor(c => c.User, userFaker)
            .RuleFor(c => c.Role, f => f.Name.JobTitle());

        MockRepository = fixture.Freeze<Mock<IUserRepository>>();
        MockLogger = fixture.Freeze<Mock<ILogger<ChangeUserRoleCommandHandler>>>();

        ChangeUserRoleCommandHandler = new ChangeUserRoleCommandHandler(
            MockRepository.Object,
            MockLogger.Object);

        SucceededResult = IdentityResult.Success;
        FailedResult = IdentityResult.Failed();
        ChangeUserRoleCommand = changeUserRoleCommandFaker.Generate();
    }

    public ChangeUserRoleCommandHandler ChangeUserRoleCommandHandler { get; }
    public Mock<IUserRepository> MockRepository { get; }
    public Mock<ILogger<ChangeUserRoleCommandHandler>> MockLogger { get; }

    public CancellationToken CancellationToken { get; }
    public IdentityResult SucceededResult { get; }
    public IdentityResult FailedResult { get; }
    public ChangeUserRoleCommand ChangeUserRoleCommand { get; }
}
