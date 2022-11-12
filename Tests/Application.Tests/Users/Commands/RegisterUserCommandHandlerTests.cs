using Application.Tests.Users.Commands.Fixtures;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Application.Tests.Users.Commands;

public class RegisterUserCommandHandlerTests : IClassFixture<RegisterUserCommandHandlerFixture>
{
    private readonly RegisterUserCommandHandlerFixture _fixture;

	public RegisterUserCommandHandlerTests(RegisterUserCommandHandlerFixture fixture)
	{
		_fixture = fixture;
    }

    [Fact]
    public async Task Handle_Should_ReturnUser_WhenEmailIsUnique()
    {
        // Arrange
        _fixture.MockRepository
            .Setup(r => r.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(_fixture.SucceededResult);

        _fixture.MockRepository
            .Setup(r => r.AssignRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(_fixture.SucceededResult);

        // Act
        var result = await _fixture.RegisterUserCommandHandler.Handle(_fixture.RegisterUserCommand, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<User>();
    }

    [Fact]
    public async Task Handle_Should_ThrowValueNotUniqueException_WhenEmailIsNotUnique()
    {
        // Arrange
        _fixture.MockRepository
            .Setup(r => r.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(_fixture.FailedResult);

        // Act
        var result = async () => await _fixture.RegisterUserCommandHandler
            .Handle(_fixture.RegisterUserCommand, _fixture.CancellationToken);

        // Assert
        await result.Should().ThrowAsync<ValueNotUniqueException>();
    }

    [Fact]
    public async Task Handle_Should_ThrowOperationFailedException_WhenRoleAssignmentFailed()
    {
        // Arrange
        _fixture.MockRepository
            .Setup(r => r.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(_fixture.SucceededResult);

        _fixture.MockRepository
            .Setup(r => r.AssignRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(_fixture.FailedResult);

        // Act
        var result = async () => await _fixture.RegisterUserCommandHandler
            .Handle(_fixture.RegisterUserCommand, _fixture.CancellationToken);

        // Assert
        await result.Should().ThrowAsync<OperationFailedException>();
    }
}
