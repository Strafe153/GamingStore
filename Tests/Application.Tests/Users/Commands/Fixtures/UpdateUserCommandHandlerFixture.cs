using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Users.Commands.Update;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Users.Commands.Fixtures;

public class UpdateUserCommandHandlerFixture
{
    public UpdateUserCommandHandlerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockRepository = fixture.Freeze<Mock<IUserRepository>>();
        MockUserService = fixture.Freeze<Mock<IUserService>>();
        MockPictureService = fixture.Freeze<Mock<IPictureService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<UpdateUserCommandHandler>>>();

        UpdateUserCommandHandler = new UpdateUserCommandHandler(
            MockRepository.Object,
            MockUserService.Object,
            MockPictureService.Object,
            MockLogger.Object);

        Name = "Name";
        SucceededResult = IdentityResult.Success;
        FailedResult = IdentityResult.Failed();
        User = GetUser();
        UpdateUserCommand = GetUpdateUserCommand();
    }

    public UpdateUserCommandHandler UpdateUserCommandHandler { get; }
    public Mock<IUserRepository> MockRepository { get; }
    public Mock<IUserService> MockUserService { get; }
    public Mock<IPictureService> MockPictureService { get; }
    public Mock<ILogger<UpdateUserCommandHandler>> MockLogger { get; }

    public string Name { get; }
    public IFormFile? ProfilePicture { get; }
    public CancellationToken CancellationToken { get; }
    public IdentityResult SucceededResult { get; }
    public IdentityResult FailedResult { get; }
    public User User { get; }
    public UpdateUserCommand UpdateUserCommand { get; }

    private User GetUser()
    {
        return new User(Name, Name, Name, Name, Name, Name);
    }

    private UpdateUserCommand GetUpdateUserCommand()
    {
        return new UpdateUserCommand()
        {
            User = User,
            FirstName = Name,
            LastName = Name,
            PhoneNumber = Name,
            ProfilePicture = ProfilePicture
        };
    }
}
