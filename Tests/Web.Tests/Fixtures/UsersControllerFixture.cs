using Application.Users.Commands.ChangePassword;
using Application.Users.Commands.ChangeRole;
using Application.Users.Commands.Register;
using Application.Users.Commands.Update;
using Application.Users.Queries;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Domain.Entities;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Presentation.Controllers;

namespace Presentation.Tests.Fixtures;

public class UsersControllerFixture
{
	public UsersControllerFixture()
	{
		var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockSender = fixture.Freeze<Mock<ISender>>();
        MockMapper = fixture.Freeze<Mock<IMapper>>();

        UsersController = new UsersController(
            MockSender.Object,
            MockMapper.Object);

        Id = 1;
        Name = "Name";
        PageParameters = GetPageParameters();
        User = GetUser();
        GetUserResponse = GetGetUserResponse();
        RegisterUserRequest = GetRegisterUserRequest();
        UpdateDeviceRequest = GetUpdateUserRequest();
        UpdateUserCommand = GetUpdateUserCommand();
        ChangeUserPasswordRequest = GetChangeUserPasswordRequest();
        ChangeUserPasswordCommand = GetChangeUserPasswordCommand();
        ChangeUserRoleRequest = GetChangeUserRoleRequest();
        ChangeUserRoleCommand = GetChangeUserRoleCommand();
        PaginatedList = GetPaginatedList();
        PaginatedModel = GetPaginatedModel();
    }

    public UsersController UsersController { get; }
    public Mock<ISender> MockSender { get; }
    public Mock<IMapper> MockMapper { get; }

    public int Id { get; }
    public string Name { get; }
    public IFormFile? ProfilePicture { get; }
    public Unit Unit { get; }
    public PageParameters PageParameters { get; set; }
    public CancellationToken CancellationToken { get; }
    public User User { get; }
    public GetUserResponse GetUserResponse { get; }
    public RegisterUserRequest RegisterUserRequest { get; }
    public UpdateUserRequest UpdateDeviceRequest { get; }
    public UpdateUserCommand UpdateUserCommand { get; }
    public ChangeUserPasswordRequest ChangeUserPasswordRequest { get; }
    public ChangeUserPasswordCommand ChangeUserPasswordCommand { get; }
    public ChangeUserRoleRequest ChangeUserRoleRequest { get; }
    public ChangeUserRoleCommand ChangeUserRoleCommand { get; }
    public PaginatedList<User> PaginatedList { get; }
    public PaginatedModel<GetUserResponse> PaginatedModel { get; }

    private PageParameters GetPageParameters()
    {
        return new PageParameters()
        {
            PageNumber = 1,
            PageSize = 5
        };
    }

    private User GetUser()
    {
        return new User(Name, Name, Name, Name, Name, Name);
    }

    private List<User> GetUsers()
    {
        return new List<User>()
        {
            User,
            User
        };
    }

    private GetUserResponse GetGetUserResponse()
    {
        return new GetUserResponse(Id, Name, Name, Name, Name, Name);
    }

    private List<GetUserResponse> GetUserResponses()
    {
        return new List<GetUserResponse>()
        {
            GetUserResponse,
            GetUserResponse
        };
    }

    private RegisterUserRequest GetRegisterUserRequest()
    {
        return new RegisterUserRequest(Name, Name, Name, Name, Name, ProfilePicture);
    }

    private UpdateUserRequest GetUpdateUserRequest()
    {
        return new UpdateUserRequest(Name, Name, Name, ProfilePicture);
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

    private ChangeUserPasswordRequest GetChangeUserPasswordRequest()
    {
        return new ChangeUserPasswordRequest(Name, Name);
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

    private ChangeUserRoleRequest GetChangeUserRoleRequest()
    {
        return new ChangeUserRoleRequest(Name);
    }

    private ChangeUserRoleCommand GetChangeUserRoleCommand()
    {
        return new ChangeUserRoleCommand()
        {
            User = User,
            Role = Name
        };
    }

    private PaginatedList<User> GetPaginatedList()
    {
        return new PaginatedList<User>(GetUsers(), 5, 1, 5);
    }

    private PaginatedModel<GetUserResponse> GetPaginatedModel()
    {
        return new PaginatedModel<GetUserResponse>()
        {
            CurrentPage = 1,
            TotalPages = 2,
            PageSize = 5,
            TotalItems = 6,
            HasPrevious = false,
            HasNext = true,
            Entities = GetUserResponses()
        };
    }
}
