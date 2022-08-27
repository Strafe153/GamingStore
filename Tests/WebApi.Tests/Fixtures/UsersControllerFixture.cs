using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Services;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.UserViewModels;
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
            UserBaseViewModel = GetUserBaseViewModel();
            UserReadViewModel = GetUserReadViewModel();
            UserAuthorizeViewModel = GetUserAuthorizeViewModel();
            UserChangeRoleViewModel = GetUserChangeRoleViewModel();
            UserChangePasswordViewModel = GetUserChangePasswordViewModel();
            UserWithTokenReadViewModel = GetUserWithTokenReadViewModel();
            PageParameters = GetPageParameters();
            UserPaginatedList = GetUserPaginatedList();
            UserPageViewModel = GetUserPageViewModel();
        }

        public UsersController MockUsersController { get; }
        public Mock<IUserService> MockUserService { get; }
        public Mock<IPasswordService> MockPasswordService { get; }
        public Mock<IMapper> MockMapper { get; }

        public int Id { get; }
        public string Username { get; }
        public byte[] Bytes { get; }
        public User User { get; }
        public UserBaseViewModel UserBaseViewModel { get; }
        public UserReadViewModel UserReadViewModel { get; }
        public UserAuthorizeViewModel UserAuthorizeViewModel { get; }
        public UserChangeRoleViewModel UserChangeRoleViewModel { get; }
        public UserChangePasswordViewModel UserChangePasswordViewModel { get; }
        public UserWithTokenReadViewModel UserWithTokenReadViewModel { get; }
        public PageParameters PageParameters { get; }
        public PaginatedList<User> UserPaginatedList { get; }
        public PageViewModel<UserReadViewModel> UserPageViewModel { get; }

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

        private UserBaseViewModel GetUserBaseViewModel()
        {
            return new UserBaseViewModel()
            {
                Username = Username
            };
        }

        private UserReadViewModel GetUserReadViewModel()
        {
            return new UserReadViewModel()
            {
                Id = Id,
                Username = Username,
                Role = UserRole.User
            };
        }

        private UserAuthorizeViewModel GetUserAuthorizeViewModel()
        {
            return new UserAuthorizeViewModel()
            {
                Username = Username,
                Password = Username
            };
        }

        private UserChangeRoleViewModel GetUserChangeRoleViewModel()
        {
            return new UserChangeRoleViewModel()
            {
                Role = UserRole.Admin
            };
        }

        private UserChangePasswordViewModel GetUserChangePasswordViewModel()
        {
            return new UserChangePasswordViewModel()
            {
                Password = Username
            };
        }

        private UserWithTokenReadViewModel GetUserWithTokenReadViewModel()
        {
            return new UserWithTokenReadViewModel()
            {
                Id = Id,
                Username = Username,
                Role = UserRole.User,
                Token = Username
            };
        }

        private List<UserReadViewModel> GetUserReadViewModels()
        {
            return new List<UserReadViewModel>()
            {
                UserReadViewModel,
                UserReadViewModel
            };
        }

        private PageViewModel<UserReadViewModel> GetUserPageViewModel()
        {
            return new PageViewModel<UserReadViewModel>()
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
