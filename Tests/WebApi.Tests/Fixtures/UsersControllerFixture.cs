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
            MockMapper = fixture.Freeze<Mock<IMapper>>();

            MockUsersController = new(
                MockUserService.Object,
                MockPasswordService.Object,
                MockMapper.Object);

            Id = 1;
            Username = "Username";
            Bytes = new byte[0];
            User = GetUser();
            UserBaseDto = GetUserBaseDto();
            UserReadDto = GetUserReadDto();
            UserAuthorizeDto = GetUserAuthorizeDto();
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
        public Mock<IMapper> MockMapper { get; }

        public int Id { get; }
        public string Username { get; }
        public byte[] Bytes { get; }
        public User User { get; }
        public UserBaseDto UserBaseDto { get; }
        public UserReadDto UserReadDto { get; }
        public UserAuthorizeDto UserAuthorizeDto { get; }
        public UserChangeRoleDto UserChangeRoleDto { get; }
        public UserChangePasswordDto UserChangePasswordDto { get; }
        public UserWithTokenReadDto UserWithTokenReadDto { get; }
        public PageParameters PageParameters { get; }
        public PaginatedList<User> UserPaginatedList { get; }
        public PageDto<UserReadDto> UserPageDto { get; }

        public void MockControllerBaseUser()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            MockUsersController.ControllerContext = new ControllerContext();
            MockUsersController.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                User = user
            };
        }

        private User GetUser()
        {
            return new User()
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
            return new List<User>()
            {
                GetUser(),
                GetUser()
            };
        }

        private PageParameters GetPageParameters()
        {
            return new PageParameters()
            {
                PageNumber = 1,
                PageSize = 5
            };
        }

        private PaginatedList<User> GetUserPaginatedList()
        {
            return new PaginatedList<User>(GetUsers(), 6, 1, 5);
        }

        private UserBaseDto GetUserBaseDto()
        {
            return new UserBaseDto()
            {
                Username = Username
            };
        }

        private UserReadDto GetUserReadDto()
        {
            return new UserReadDto()
            {
                Id = Id,
                Username = Username,
                Role = UserRole.User
            };
        }

        private UserAuthorizeDto GetUserAuthorizeDto()
        {
            return new UserAuthorizeDto()
            {
                Username = Username,
                Password = Username
            };
        }

        private UserChangeRoleDto GetUserChangeRoleDto()
        {
            return new UserChangeRoleDto()
            {
                Role = UserRole.Admin
            };
        }

        private UserChangePasswordDto GetUserChangePasswordDto()
        {
            return new UserChangePasswordDto()
            {
                Password = Username
            };
        }

        private UserWithTokenReadDto GetUserWithTokenReadDto()
        {
            return new UserWithTokenReadDto()
            {
                Id = Id,
                Username = Username,
                Role = UserRole.User,
                Token = Username
            };
        }

        private List<UserReadDto> GetUserReadViewModels()
        {
            return new List<UserReadDto>()
            {
                UserReadDto,
                UserReadDto
            };
        }

        private PageDto<UserReadDto> GetUserPageDto()
        {
            return new PageDto<UserReadDto>()
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
