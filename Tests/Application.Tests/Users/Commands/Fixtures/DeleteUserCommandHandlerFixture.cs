using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Users.Commands.Delete;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
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

        var userFaker = new Faker<User>()
            .CustomInstantiator(f => new(
                f.Name.FirstName(),
                f.Name.LastName(),
                f.Internet.Email(),
                f.Internet.UserName(),
                f.Phone.PhoneNumber(),
                null));

        var deleteUserCommandFaker = new Faker<DeleteUserCommand>()
            .CustomInstantiator(f => new(userFaker.Generate()));

        MockRepository = fixture.Freeze<Mock<IUserRepository>>();
        MockUserService = fixture.Freeze<Mock<IUserService>>();
        MockPictureService = fixture.Freeze<Mock<IPictureService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<DeleteUserCommandHandler>>>();

        DeleteUserCommandHandler = new DeleteUserCommandHandler(
            MockRepository.Object,
            MockUserService.Object,
            MockPictureService.Object,
            MockLogger.Object);

        SucceededResult = IdentityResult.Success;
        FailedResult = IdentityResult.Failed();
        DeleteUserCommand = deleteUserCommandFaker.Generate();
    }
    public DeleteUserCommandHandler DeleteUserCommandHandler { get; }
    public Mock<IUserRepository> MockRepository { get; }
    public Mock<IUserService> MockUserService { get; }
    public Mock<IPictureService> MockPictureService { get; }
    public Mock<ILogger<DeleteUserCommandHandler>> MockLogger { get; }

    public CancellationToken CancellationToken { get; }
    public IdentityResult SucceededResult { get; }
    public IdentityResult FailedResult { get; }
    public DeleteUserCommand DeleteUserCommand { get; }
}
