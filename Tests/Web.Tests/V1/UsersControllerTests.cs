using Application.Users.Commands.Register;
using Application.Users.Queries;
using Application.Users.Queries.GetAll;
using Application.Users.Queries.GetById;
using Domain.Shared.Paging;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Tests.V1.Fixtures;
using Xunit;

namespace Web.Tests.V1;

public class UsersControllerTests : IClassFixture<UsersControllerFixture>
{
    private readonly UsersControllerFixture _fixture;

    public UsersControllerTests(UsersControllerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Get_Should_ReturnActionResultOfPagedModelOfGetUserResponse_WhenDataIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetAllUsersQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.PagedList);

        // Act
        var result = await _fixture.UsersController.Get(_fixture.PageParameters, _fixture.CancellationToken);
        var objectResult = result.Result.As<OkObjectResult>();
        var pagedModel = objectResult.Value.As<PagedModel<GetUserResponse>>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<PagedModel<GetUserResponse>>>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        pagedModel.Entities.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Get_Should_ReturnAcitonResultOfGetDeviceResponse_WhenUserExists()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetUserByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.User);

        // Act
        var result = await _fixture.UsersController.Get(_fixture.Id, _fixture.CancellationToken);
        var objectResult = result.Result.As<OkObjectResult>();
        var getCompanyResponse = objectResult.Value.As<GetUserResponse>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<GetUserResponse>>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        getCompanyResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task Register_Should_ReturnActionResultOfGetUserResponse_WhenRequestIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<RegisterUserCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.User);

        // Act
        var result = await _fixture.UsersController.Register(_fixture.RegisterUserRequest, _fixture.CancellationToken);
        var objectResult = result.Result.As<CreatedAtActionResult>();
        var getCompanyResponse = objectResult.Value.As<GetUserResponse>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<GetUserResponse>>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        getCompanyResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task Update_Should_ReturnNoContentResult_WhenUserExistsAndRequestIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetUserByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.User);

        // Act
        var result = await _fixture.UsersController
            .Update(_fixture.Id, _fixture.UpdateUserRequest, _fixture.CancellationToken);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task ChangePassword_Should_ReturnNoContentResult_WhenUserExistsAndRequestIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetUserByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.User);

        // Act
        var result = await _fixture.UsersController
            .ChangePassword(_fixture.Id, _fixture.ChangeUserPasswordRequest, _fixture.CancellationToken);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task ChangeRole_Should_ReturnNoContentResult_WhenUserExistsAndRequestIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetUserByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.User);

        // Act
        var result = await _fixture.UsersController
            .ChangeRole(_fixture.Id, _fixture.ChangeUserRoleRequest, _fixture.CancellationToken);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task Delete_Should_ReturnNoContentResult_WhenUserExists()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetUserByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.User);

        // Act
        var result = await _fixture.UsersController.Delete(_fixture.Id, _fixture.CancellationToken);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
}
