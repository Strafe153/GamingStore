using Application.Users.Commands.ChangePassword;
using Application.Users.Commands.ChangeRole;
using Application.Users.Commands.Register;
using Application.Users.Commands.Update;
using Application.Users.Queries;
using Application.Users.Queries.GetAll;
using Application.Users.Queries.GetById;
using Domain.Entities;
using Domain.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Tests.Fixtures;
using Xunit;

namespace Presentation.Tests;

public class UsersControllerTests : IClassFixture<UsersControllerFixture>
{
    private readonly UsersControllerFixture _fixture;

	public UsersControllerTests(UsersControllerFixture fixture)
	{
		_fixture = fixture;
    }

    [Fact]
    public async Task GetAsync_Should_ReturnActionResultOfPaginatedModelOfGetUserResponse_WhenDataIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetAllUsersQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.PaginatedList);

        _fixture.MockMapper
            .Setup(m => m.Map<PaginatedModel<GetUserResponse>>(It.IsAny<PaginatedList<User>>()))
            .Returns(_fixture.PaginatedModel);

        // Act
        var result = await _fixture.UsersController.GetAsync(_fixture.PageParameters, _fixture.CancellationToken);
        var objectResult = result.Result.As<OkObjectResult>();
        var paginatedModel = objectResult.Value.As<PaginatedModel<GetUserResponse>>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<PaginatedModel<GetUserResponse>>>();
        objectResult.StatusCode.Should().Be(200);
        paginatedModel.Entities.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAsync_Should_ReturnAcitonResultOfGetDeviceResponse_WhenUserExists()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetUserByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.User);

        _fixture.MockMapper
            .Setup(m => m.Map<GetUserResponse>(It.IsAny<User>()))
            .Returns(_fixture.GetUserResponse);

        // Act
        var result = await _fixture.UsersController.GetAsync(_fixture.Id, _fixture.CancellationToken);
        var objectResult = result.Result.As<OkObjectResult>();
        var getCompanyResponse = objectResult.Value.As<GetUserResponse>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<GetUserResponse>>();
        objectResult.StatusCode.Should().Be(200);
        getCompanyResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task RegisterAsync_Should_ReturnActionResultOfGetUserResponse_WhenRequestIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<RegisterUserCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.User);

        _fixture.MockMapper
            .Setup(m => m.Map<GetUserResponse>(It.IsAny<User>()))
            .Returns(_fixture.GetUserResponse);

        // Act
        var result = await _fixture.UsersController.RegisterAsync(_fixture.RegisterUserRequest, _fixture.CancellationToken);
        var objectResult = result.Result.As<CreatedAtActionResult>();
        var getCompanyResponse = objectResult.Value.As<GetUserResponse>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<GetUserResponse>>();
        objectResult.StatusCode.Should().Be(201);
        getCompanyResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_ReturnNoContentResult_WhenUserExistsAndRequestIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetUserByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.User);

        _fixture.MockMapper
            .Setup(m => m.Map<UpdateUserCommand>(It.IsAny<UpdateUserRequest>()))
            .Returns(_fixture.UpdateUserCommand);

        // Act
        var result = await _fixture.UsersController
            .UpdateAsync(_fixture.Id, _fixture.UpdateDeviceRequest, _fixture.CancellationToken);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task ChangePasswordAsync_Should_ReturnNoContentResult_WhenUserExistsAndRequestIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetUserByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.User);

        _fixture.MockMapper
            .Setup(m => m.Map<ChangeUserPasswordCommand>(It.IsAny<ChangeUserPasswordRequest>()))
            .Returns(_fixture.ChangeUserPasswordCommand);

        // Act
        var result = await _fixture.UsersController
            .ChangePasswordAsync(_fixture.Id, _fixture.ChangeUserPasswordRequest, _fixture.CancellationToken);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task ChangeRoleAsync_Should_ReturnNoContentResult_WhenUserExistsAndRequestIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetUserByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.User);

        _fixture.MockMapper
            .Setup(m => m.Map<ChangeUserRoleCommand>(It.IsAny<ChangeUserRoleRequest>()))
            .Returns(_fixture.ChangeUserRoleCommand);

        // Act
        var result = await _fixture.UsersController
            .ChangeRoleAsync(_fixture.Id, _fixture.ChangeUserRoleRequest, _fixture.CancellationToken);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task DeleteAsync_Should_ReturnNoContentResult_WhenUserExists()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetUserByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.User);

        // Act
        var result = await _fixture.UsersController.DeleteAsync(_fixture.Id, _fixture.CancellationToken);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.Should().NotBeNull();
    }
}
