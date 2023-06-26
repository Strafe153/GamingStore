using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Users.Commands.Register;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Users.Commands.Fixtures;

public class RegisterUserCommandHandlerFixture
{
    public RegisterUserCommandHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var registerUserCommandFaker = new Faker<RegisterUserCommand>()
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(c => c.Password, f => f.Internet.Password())
            .RuleFor(c => c.UserName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName));

        MockRepository = fixture.Freeze<Mock<IUserRepository>>();
        MockPictureService = fixture.Freeze<Mock<IPictureService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<RegisterUserCommandHandler>>>();

        RegisterUserCommandHandler = new RegisterUserCommandHandler(
            MockRepository.Object,
            MockPictureService.Object,
            MockLogger.Object);

        SucceededResult = IdentityResult.Success;
        FailedResult = IdentityResult.Failed();
        RegisterUserCommand = registerUserCommandFaker.Generate();
    }

    public RegisterUserCommandHandler RegisterUserCommandHandler { get; }
    public Mock<IUserRepository> MockRepository { get; }
    public Mock<IPictureService> MockPictureService { get; }
    public Mock<ILogger<RegisterUserCommandHandler>> MockLogger { get; }

    public CancellationToken CancellationToken { get; }
    public IdentityResult SucceededResult { get; }
    public IdentityResult FailedResult { get; }
    public RegisterUserCommand RegisterUserCommand { get; }
}
