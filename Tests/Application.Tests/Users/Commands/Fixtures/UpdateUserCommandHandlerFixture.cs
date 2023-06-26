using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Users.Commands.Update;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Users.Commands.Fixtures;

public class UpdateUserCommandHandlerFixture
{
    public UpdateUserCommandHandlerFixture()
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

        var updateUserCommandFaker = new Faker<UpdateUserCommand>()
            .RuleFor(c => c.User, userFaker)
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber());

        MockRepository = fixture.Freeze<Mock<IUserRepository>>();
        MockUserService = fixture.Freeze<Mock<IUserService>>();
        MockPictureService = fixture.Freeze<Mock<IPictureService>>();
        MockLogger = fixture.Freeze<Mock<ILogger<UpdateUserCommandHandler>>>();

        UpdateUserCommandHandler = new UpdateUserCommandHandler(
            MockRepository.Object,
            MockUserService.Object,
            MockPictureService.Object,
            MockLogger.Object);

        SucceededResult = IdentityResult.Success;
        FailedResult = IdentityResult.Failed();
        UpdateUserCommand = updateUserCommandFaker.Generate();
    }

    public UpdateUserCommandHandler UpdateUserCommandHandler { get; }
    public Mock<IUserRepository> MockRepository { get; }
    public Mock<IUserService> MockUserService { get; }
    public Mock<IPictureService> MockPictureService { get; }
    public Mock<ILogger<UpdateUserCommandHandler>> MockLogger { get; }

    public CancellationToken CancellationToken { get; }
    public IdentityResult SucceededResult { get; }
    public IdentityResult FailedResult { get; }
    public UpdateUserCommand UpdateUserCommand { get; }
}
