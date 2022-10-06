using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Core.Dtos;
using Core.Dtos.UserDtos;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using WebApi.Controllers;

namespace WebApi.Tests.Fixtures;

public class UsersControllerFixture
{
    public UsersControllerFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockUserService = fixture.Freeze<Mock<IUserService>>();
        MockPictureService = fixture.Freeze<Mock<IPictureService>>();
        MockMapper = fixture.Freeze<Mock<IMapper>>();

        MockUsersController = new(
            MockUserService.Object,
            MockPictureService.Object,
            MockMapper.Object);

        Id = 1;
        Name = "Name";
        Bytes = new byte[0];
        User = GetUser();
        UserBaseDto = GetUserBaseDto();
        UserReadDto = GetUserReadDto();
        UserRegisterDto = GetUserRegisterDto();
        UserLoginDto = GetUserLoginDto();
        UserUpdateDto = GetUserUpdateDto();
        UserChangeRoleDto = GetUserChangeRoleDto();
        UserChangePasswordDto = GetUserChangePasswordDto();
        PageParameters = GetPageParameters();
        UserPaginatedList = new(GetUsers(), 6, 1, 5);
        UserPageDto = GetUserPageDto();
    }

    public UsersController MockUsersController { get; }
    public Mock<IUserService> MockUserService { get; }
    public Mock<IPictureService> MockPictureService { get; }
    public Mock<IMapper> MockMapper { get; }

    public int Id { get; }
    public string Name { get; }
    public byte[] Bytes { get; }
    public IFormFile? Picture { get; }
    public User User { get; }
    public UserBaseDto UserBaseDto { get; }
    public UserReadDto UserReadDto { get; }
    public UserRegisterDto UserRegisterDto { get; }
    public UserLoginDto UserLoginDto { get; }
    public UserUpdateDto UserUpdateDto { get; }
    public UserChangeRoleDto UserChangeRoleDto { get; }
    public UserChangePasswordDto UserChangePasswordDto { get; }
    public PageParameters PageParameters { get; }
    public PaginatedList<User> UserPaginatedList { get; }
    public PageDto<UserReadDto> UserPageDto { get; }
    public CancellationToken CancellationToken { get; }

    private User GetUser()
    {
        return new()
        {
            Id = Id,
            UserName = Name,
            PasswordHash = Name,
        };
    }

    private List<User> GetUsers()
    {
        return new()
        {
            GetUser(),
            GetUser()
        };
    }

    private PageParameters GetPageParameters()
    {
        return new()
        {
            PageNumber = 1,
            PageSize = 5
        };
    }

    private UserBaseDto GetUserBaseDto()
    {
        return new()
        {
            FirstName = Name,
            LastName = Name
        };
    }

    private UserReadDto GetUserReadDto()
    {
        return new()
        {
            Id = Id,
            FirstName = Name,
            LastName = Name
        };
    }

    private UserRegisterDto GetUserRegisterDto()
    {
        return new()
        {
            FirstName = Name,
            LastName = Name,
            Password = Name
        };
    }

    private UserLoginDto GetUserLoginDto()
    {
        return new()
        {
            Email = Name,
            Password = Name
        };
    }

    private UserUpdateDto GetUserUpdateDto()
    {
        return new()
        {
            FirstName = Name,
            LastName = Name,
            ProfilePicture = Picture
        };
    }

    private UserChangeRoleDto GetUserChangeRoleDto()
    {
        return new()
        {
            Role = Name
        };
    }

    private UserChangePasswordDto GetUserChangePasswordDto()
    {
        return new()
        {
            CurrentPassword = Name,
            NewPassword = Name
        };
    }

    private List<UserReadDto> GetUserReadViewModels()
    {
        return new()
        {
            UserReadDto,
            UserReadDto
        };
    }

    private PageDto<UserReadDto> GetUserPageDto()
    {
        return new()
        {
            CurrentPage = 1,
            TotalPages = 2,
            PageSize = 5,
            TotalItems = 6,
            HasPrevious = false,
            HasNext = true,
            Entities = GetUserReadViewModels()
        };
    }
}
