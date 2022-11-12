using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Users.Commands.Register;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Users.Commands.Fixtures;

public class RegisterUserCommandHandlerFixture
{
    public RegisterUserCommandHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockRepository = fixture.Freeze<Mock<IUserRepository>>();
        MockPictureService = fixture.Freeze<Mock<IPictureService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<RegisterUserCommandHandler>>>();

        RegisterUserCommandHandler = new RegisterUserCommandHandler(
            MockRepository.Object,
            MockPictureService.Object,
            MockLogger.Object);

        Name = "Name";
        SucceededResult = IdentityResult.Success;
        FailedResult = IdentityResult.Failed();
        RegisterUserCommand = GetRegisterUserCommand();
    }

    public RegisterUserCommandHandler RegisterUserCommandHandler { get; }
    public Mock<IUserRepository> MockRepository { get; }
    public Mock<IPictureService> MockPictureService { get; }
    public Mock<ILogger<RegisterUserCommandHandler>> MockLogger { get; }

    public string Name { get; }
    public IFormFile? Picture { get; }
    public CancellationToken CancellationToken { get; }
    public IdentityResult SucceededResult { get; }
    public IdentityResult FailedResult { get; }
    public RegisterUserCommand RegisterUserCommand { get; }

    private RegisterUserCommand GetRegisterUserCommand()
    {
        return new RegisterUserCommand()
        {
            FirstName = Name,
            LastName = Name,
            Email = Name,
            UserName = Name,
            PhoneNumber = Name,
            Password = Name,
            ProfilePicture = Picture
        };
    }
}
