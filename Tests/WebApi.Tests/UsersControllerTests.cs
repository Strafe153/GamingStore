using Core.Dtos;
using Core.Dtos.UserDtos;
using Core.Entities;
using Core.Models;
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
        public async Task GetAsync_ValidPageParameters_ReturnsActionResultOfPageDtoOfUserReadDto()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(_fixture.UserPaginatedList);

            _fixture.MockMapper
                .Setup(m => m.Map<PageDto<UserReadDto>>(It.IsAny<PaginatedList<User>>()))
                .Returns(_fixture.UserPageDto);

            // Act
            var result = await _fixture.MockUsersController.GetAsync(_fixture.PageParameters);
            var pageDto = result.Result.As<OkObjectResult>().Value.As<PageDto<UserReadDto>>();

            // Assert
            result.Should().NotBeNull().And.BeOfType<ActionResult<PageDto<UserReadDto>>>();
            pageDto.Entities.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetAsync_ExistingUser_ReturnsActionResultOfUserReadDto()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.User);

            _fixture.MockMapper
                .Setup(m => m.Map<UserReadDto>(It.IsAny<User>()))
                .Returns(_fixture.UserReadDto);

            // Act
            var result = await _fixture.MockUsersController.GetAsync(_fixture.Id);
            var readDto = result.Result.As<OkObjectResult>().Value.As<UserReadDto>();

            // Assert
            result.Should().NotBeNull().And.BeOfType<ActionResult<UserReadDto>>();
            readDto.Should().NotBeNull();
        }

        [Fact]
        public async Task RegisterAsync_ValidDto_ReturnsActionResultOfUserWithTokenReadDto()
        {
            // Arrange
            _fixture.MockMapper
                .Setup(m => m.Map<UserReadDto>(It.IsAny<User>()))
                .Returns(_fixture.UserReadDto);

            // Act
            var result = await _fixture.MockUsersController.RegisterAsync(_fixture.UserRegisterDto);
            var readDto = result.Result.As<CreatedAtActionResult>().Value.As<UserReadDto>();

            // Assert
            result.Should().NotBeNull().And.BeOfType<ActionResult<UserReadDto>>();
            readDto.Should().NotBeNull();
        }

        [Fact]
        public async Task LoginAsync_ValidDto_ReturnsActionResultOfUserWithTokenReadDto()
        {
            // Arrange
            _fixture.MockMapper
                .Setup(m => m.Map<UserWithTokenReadDto>(It.IsAny<User>()))
                .Returns(_fixture.UserWithTokenReadDto);

            // Act
            var result = await _fixture.MockUsersController.LoginAsync(_fixture.UserLoginDto);
            var readWithTokenDto = result.Result.As<OkObjectResult>().Value.As<UserWithTokenReadDto>();

            // Assert
            result.Should().NotBeNull().And.BeOfType<ActionResult<UserWithTokenReadDto>>();
            readWithTokenDto.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateAsync_ExistingUserValidDto_ReturnsNoContentResult()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.User);

            // Act
            var result = await _fixture.MockUsersController.UpdateAsync(_fixture.Id, _fixture.UserBaseDto);

            // Assert
            result.Should().NotBeNull().And.BeOfType<NoContentResult>();
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
            var result = await _fixture.MockUsersController.ChangePasswordAsync(_fixture.Id, _fixture.UserChangePasswordDto);
            var readToken = result.Result.As<OkObjectResult>().Value.As<string>();

            // Assert
            result.Should().NotBeNull().And.BeOfType<ActionResult<string>>();
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
            var result = await _fixture.MockUsersController.ChangeRoleAsync(_fixture.Id, _fixture.UserChangeRoleDto);

            // Assert
            result.Should().NotBeNull().And.BeOfType<NoContentResult>();
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
            result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        }
    }
}
