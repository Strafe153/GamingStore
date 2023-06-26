using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Users.Commands.ChangePassword;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Users.Commands.Fixtures;

public class ChangeUserPasswordCommandHandlerFixture
{
    public ChangeUserPasswordCommandHandlerFixture()
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

        var changeUserPasswordCommandFaker = new Faker<ChangeUserPasswordCommand>()
            .RuleFor(c => c.User, userFaker)
            .RuleFor(c => c.CurrentPassword, f => f.Internet.Password())
            .RuleFor(c => c.NewPassword, f => f.Internet.Password());

        MockRepository = fixture.Freeze<Mock<IUserRepository>>();
        MockUserService = fixture.Freeze<Mock<IUserService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<ChangeUserPasswordCommandHandler>>>();

        ChangeUserPasswordCommandHandler = new ChangeUserPasswordCommandHandler(
            MockRepository.Object,
            MockUserService.Object,
            MockLogger.Object);

        SucceededResult = IdentityResult.Success;
        FailedResult = IdentityResult.Failed();
        ChangeUserPasswordCommand = changeUserPasswordCommandFaker.Generate();
    }

    public ChangeUserPasswordCommandHandler ChangeUserPasswordCommandHandler { get; }
    public Mock<IUserRepository> MockRepository { get; }
    public Mock<IUserService> MockUserService { get; }
    public Mock<ILogger<ChangeUserPasswordCommandHandler>> MockLogger { get; }

    public CancellationToken CancellationToken { get; }
    public IdentityResult SucceededResult { get; }
    public IdentityResult FailedResult { get; }
    public ChangeUserPasswordCommand ChangeUserPasswordCommand { get; }
}
