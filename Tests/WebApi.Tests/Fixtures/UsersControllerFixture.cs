using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Core.Dtos;
using Core.Dtos.UserDtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using WebApi.Controllers;

namespace WebApi.Tests.Fixtures
{
    public class UsersControllerFixture
    {
        public UsersControllerFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockUserService = fixture.Freeze<Mock<IUserService>>();
            MockPasswordService = fixture.Freeze<Mock<IPasswordService>>();
            MockPictureService = fixture.Freeze<Mock<IPictureService>>();
            MockMapper = fixture.Freeze<Mock<IMapper>>();

            MockUsersController = new(
                MockUserService.Object,
                MockPasswordService.Object,
                MockPictureService.Object,
                MockMapper.Object);

            Id = 1;
            Username = "Username";
            Bytes = new byte[0];
            User = GetUser();
            UserBaseDto = GetUserBaseDto();
            UserReadDto = GetUserReadDto();
            UserRegisterDto = GetUserRegisterDto();
            UserLoginDto = GetUserLoginDto();
            UserUpdateDto = GetUserUpdateDto();
            UserChangeRoleDto = GetUserChangeRoleDto();
            UserChangePasswordDto = GetUserChangePasswordDto();
            UserWithTokenReadDto = GetUserWithTokenReadDto();
            PageParameters = GetPageParameters();
            UserPaginatedList = GetUserPaginatedList();
            UserPageDto = GetUserPageDto();
        }

        public UsersController MockUsersController { get; }
        public Mock<IUserService> MockUserService { get; }
        public Mock<IPasswordService> MockPasswordService { get; }
        public Mock<IPictureService> MockPictureService { get; }
        public Mock<IMapper> MockMapper { get; }

        public int Id { get; }
        public string Username { get; }
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
        public UserWithTokenReadDto UserWithTokenReadDto { get; }
        public PageParameters PageParameters { get; }
        public PaginatedList<User> UserPaginatedList { get; }
        public PageDto<UserReadDto> UserPageDto { get; }

        public void MockControllerBaseUser()
        {
            ClaimsPrincipal user = new(new ClaimsIdentity());

            MockUsersController.ControllerContext = new ControllerContext();
            MockUsersController.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                User = user
            };
        }

        private User GetUser()
        {
            return new()
            {
                Id = Id,
                Username = Username,
                Role = UserRole.User,
                PasswordHash = Bytes,
                PasswordSalt = Bytes
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

        private PaginatedList<User> GetUserPaginatedList()
        {
            return new(GetUsers(), 6, 1, 5);
        }

        private UserBaseDto GetUserBaseDto()
        {
            return new()
            {
                Username = Username
            };
        }

        private UserReadDto GetUserReadDto()
        {
            return new()
            {
                Id = Id,
                Username = Username,
                Role = UserRole.User
            };
        }

        private UserRegisterDto GetUserRegisterDto()
        {
            return new()
            {
                Username = Username,
                Password = Username
            };
        }

        private UserLoginDto GetUserLoginDto()
        {
            return new()
            {
                Email = Username,
                Password = Username
            };
        }

        private UserUpdateDto GetUserUpdateDto()
        {
            return new()
            {
                Username = Username,
                ProfilePicture = Picture
            };
        }

        private UserChangeRoleDto GetUserChangeRoleDto()
        {
            return new()
            {
                Role = UserRole.Admin
            };
        }

        private UserChangePasswordDto GetUserChangePasswordDto()
        {
            return new()
            {
                Password = Username
            };
        }

        private UserWithTokenReadDto GetUserWithTokenReadDto()
        {
            return new()
            {
                Id = Id,
                Username = Username,
                Role = UserRole.User,
                Token = Username
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
}
