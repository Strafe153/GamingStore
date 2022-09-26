using Core.Dtos;
using Core.Dtos.UserDtos;
using Core.Entities;
using Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Tests.Fixtures;
using Xunit;

namespace WebApi.Tests;

public class UsersControllerTests : IClassFixture<UsersControllerFixture>
{
    private readonly UsersControllerFixture _fixture;

    public UsersControllerTests(UsersControllerFixture fixture)
    {
        _fixture = fixture;
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
        var objectResult = result.Result.As<OkObjectResult>();
        var pageDto = objectResult.Value.As<PageDto<UserReadDto>>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<PageDto<UserReadDto>>>();
        objectResult.StatusCode.Should().Be(200);
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
        var objectResult = result.Result.As<OkObjectResult>();
        var readDto = objectResult.Value.As<UserReadDto>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<UserReadDto>>();
        objectResult.StatusCode.Should().Be(200);
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
        var objectResult = result.Result.As<CreatedAtActionResult>();
        var readDto = objectResult.Value.As<UserReadDto>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<UserReadDto>>();
        objectResult.StatusCode.Should().Be(201);
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
        var objectResult = result.Result.As<OkObjectResult>();
        var readWithTokenDto = objectResult.Value.As<UserWithTokenReadDto>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<UserWithTokenReadDto>>();
        objectResult.StatusCode.Should().Be(200);
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
        var result = await _fixture.MockUsersController.UpdateAsync(_fixture.Id, _fixture.UserUpdateDto);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(204);
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
        var objectResult = result.Result.As<OkObjectResult>();
        var readToken = objectResult.Value.As<string>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<string>>();
        objectResult.StatusCode.Should().Be(200);
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
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(204);
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
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(204);
    }
}
