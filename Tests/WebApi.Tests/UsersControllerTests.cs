using Core.Entities;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.UserViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Tests.Fixtures;
using Xunit;

namespace WebApi.Tests
{
    public class UsersControllerTests : IClassFixture<UsersControllerFixture>
    {
        private readonly UsersControllerFixture _fixture;

        public UsersControllerTests(UsersControllerFixture fixture)
        {
            _fixture = fixture;
            _fixture.MockControllerBaseUser();
        }

        [Fact]
        public async Task GetAsync_ValidPageParameters_ReturnsActionResultOfPageViewModelOfUserReadViewModel()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(_fixture.UserPaginatedList);

            _fixture.MockUserPaginatedMapper
                .Setup(m => m.Map(It.IsAny<PaginatedList<User>>()))
                .Returns(_fixture.UserPageViewModel);

            // Act
            var result = await _fixture.MockUsersController.GetAsync(_fixture.PageParameters);
            var pageViewModel = result.Result.As<OkObjectResult>().Value.As<PageViewModel<UserReadViewModel>>();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<PageViewModel<UserReadViewModel>>>();
            pageViewModel.Entities.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetAsync_ExistingUser_ReturnsActionResultOfUserReadViewModel()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.User);

            _fixture.MockUserReadMapper
                .Setup(m => m.Map(It.IsAny<User>()))
                .Returns(_fixture.UserReadViewModel);

            // Act
            var result = await _fixture.MockUsersController.GetAsync(_fixture.Id);
            var readViewModel = result.Result.As<OkObjectResult>().Value.As<UserReadViewModel>();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<UserReadViewModel>>();
            readViewModel.Should().NotBeNull();
        }

        [Fact]
        public async Task RegisterAsync_ValidViewModel_ReturnsActionResultOfUserWithTokenReadViewModel()
        {
            // Arrange
            _fixture.MockUserReadMapper
                .Setup(m => m.Map(It.IsAny<User>()))
                .Returns(_fixture.UserReadViewModel);

            // Act
            var result = await _fixture.MockUsersController.RegisterAsync(_fixture.UserAuthorizeViewModel);
            var readViewModel = result.Result.As<CreatedAtActionResult>().Value.As<UserReadViewModel>();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<UserReadViewModel>>();
            readViewModel.Should().NotBeNull();
        }

        [Fact]
        public async Task LoginAsync_ValidViewModel_ReturnsActionResultOfUserWithTokenReadViewModel()
        {
            // Arrange
            _fixture.MockUserWithTokenReadMapper
                .Setup(m => m.Map(It.IsAny<User>()))
                .Returns(_fixture.UserWithTokenReadViewModel);

            // Act
            var result = await _fixture.MockUsersController.LoginAsync(_fixture.UserAuthorizeViewModel);
            var readWithTokenViewModel = result.Result.As<OkObjectResult>().Value.As<UserWithTokenReadViewModel>();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<UserWithTokenReadViewModel>>();
            readWithTokenViewModel.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateAsync_ExistingUserValidViewModel_ReturnsNoContentResult()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.User);

            // Act
            var result = await _fixture.MockUsersController.UpdateAsync(_fixture.Id, _fixture.UserBaseViewModel);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ChangePasswordAsync_ExistingUserValidPassword_ReturnsNoContentResult()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.User);

            _fixture.MockPasswordService
                .Setup(s => s.CreateToken(It.IsAny<User>()))
                .Returns(_fixture.Username);

            // Act
            var result = await _fixture.MockUsersController
                .ChangePasswordAsync(_fixture.Id, _fixture.UserChangePasswordViewModel);
            var readToken = result.Result.As<OkObjectResult>().Value.As<string>();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<string>>();
            readToken.Should().NotBeNull();
        }

        [Fact]
        public async Task ChangeRoleAsync_ExistingUserValidRole_ReturnsNoContentResult()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.User);

            // Act
            var result = await _fixture.MockUsersController.ChangeRoleAsync(_fixture.Id, _fixture.UserChangeRoleViewModel);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteAsync_ExistingUser_ReturnsNoContentResult()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.User);

            // Act
            var result = await _fixture.MockUsersController.DeleteAsync(_fixture.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
