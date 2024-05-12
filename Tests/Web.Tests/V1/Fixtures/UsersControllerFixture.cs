using Application.Users.Commands.ChangePassword;
using Application.Users.Commands.ChangeRole;
using Application.Users.Commands.Register;
using Application.Users.Commands.Update;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Bogus;
using Domain.Entities;
using Domain.Shared.PageParameters;
using Domain.Shared.Paging;
using MediatR;
using Moq;
using Presentation.AutoMapperProfiles;
using Presentation.Controllers.V1;

namespace Presentation.Tests.V1.Fixtures;

public class UsersControllerFixture
{
    public UsersControllerFixture()
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

        var registerUserRequestFaker = new Faker<RegisterUserRequest>()
            .CustomInstantiator(f => new(
                f.Name.FirstName(),
                f.Name.LastName(),
                f.Internet.Email(),
                f.Phone.PhoneNumber(),
                f.Internet.Password(),
                null));

        var updateUserRequestFaker = new Faker<UpdateUserRequest>()
            .CustomInstantiator(f => new(
                f.Name.FirstName(),
                f.Name.LastName(),
                f.Phone.PhoneNumber(),
                null));

        var changeUserPasswordRequestFaker = new Faker<ChangeUserPasswordRequest>()
            .CustomInstantiator(f => new(
                f.Internet.Password(),
                f.Internet.Password()));

        var changeUserRoleRequestFaker = new Faker<ChangeUserRoleRequest>()
            .CustomInstantiator(f => new(f.Name.JobTitle()));

        var totalItemsCount = Random.Shared.Next(2, 50);

        var pagedListFaker = new Faker<PagedList<User>>()
            .CustomInstantiator(f => new(
                userFaker.Generate(totalItemsCount),
                totalItemsCount,
                f.Random.Int(1, 2),
                f.Random.Int(1, 2)))
            .RuleFor(l => l.PageSize, (f, l) => f.Random.Int(1, l.TotalItems))
            .RuleFor(l => l.CurrentPage, (f, l) => f.Random.Int(1, l.TotalPages));

        MockSender = fixture.Freeze<Mock<ISender>>();

        Mapper = new MapperConfiguration(options =>
        {
            options.AddProfile(new UserProfile());
        }).CreateMapper();

        UsersController = new UsersController(
            MockSender.Object,
            Mapper);

        Id = Random.Shared.Next(1, 5000);

        PageParameters = new()
        {
            PageNumber = Random.Shared.Next(1, 500),
            PageSize = Random.Shared.Next(1, 500)
        };

        User = userFaker.Generate();
        RegisterUserRequest = registerUserRequestFaker.Generate();
        UpdateUserRequest = updateUserRequestFaker.Generate();
        ChangeUserPasswordRequest = changeUserPasswordRequestFaker.Generate();
        ChangeUserRoleRequest = changeUserRoleRequestFaker.Generate();
        PagedList = pagedListFaker.Generate();
    }

    public UsersController UsersController { get; }
    public Mock<ISender> MockSender { get; }
    public IMapper Mapper { get; }

    public int Id { get; }
    public PageParameters PageParameters { get; set; }
    public CancellationToken CancellationToken { get; }
    public User User { get; }
    public RegisterUserRequest RegisterUserRequest { get; }
    public UpdateUserRequest UpdateUserRequest { get; }
    public ChangeUserPasswordRequest ChangeUserPasswordRequest { get; }
    public ChangeUserRoleRequest ChangeUserRoleRequest { get; }
    public PagedList<User> PagedList { get; }
}
