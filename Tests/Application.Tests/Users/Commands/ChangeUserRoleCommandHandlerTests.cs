using Application.Tests.Users.Commands.Fixtures;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Application.Tests.Users.Commands;

public class ChangeUserRoleCommandHandlerTests : IClassFixture<ChangeUserRoleCommandHandlerFixture>
{
    private readonly ChangeUserRoleCommandHandlerFixture _fixture;

	public ChangeUserRoleCommandHandlerTests(ChangeUserRoleCommandHandlerFixture fixture)
	{
		_fixture = fixture;
	}

    [Fact]
    public async Task Handle_Should_ReturnUnit_WhenChangeRoleSucceeded()
    {
        // Arrange
        _fixture.MockRepository
            .Setup(r => r.RemoveFromRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(_fixture.SucceededResult);

        _fixture.MockRepository
            .Setup(r => r.AssignRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(_fixture.SucceededResult);

        // Act
        var result = await _fixture.ChangeUserRoleCommandHandler.Handle(_fixture.ChangeUserRoleCommand, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<Unit>();
    }

    [Fact]
    public async Task Handle_Should_ThrowOperationFailedException_WhenRemoveFromRolesFailed()
    {
        // Arrange
        _fixture.MockRepository
            .Setup(r => r.RemoveFromRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(_fixture.FailedResult);

        // Act
        var result = async () => await _fixture.ChangeUserRoleCommandHandler
            .Handle(_fixture.ChangeUserRoleCommand, _fixture.CancellationToken);

        // Assert
        await result.Should().ThrowAsync<OperationFailedException>();
    }

    [Fact]
    public async Task Handle_Should_ThrowOperationFailedException_WhenAssignRoleFailed()
    {
        // Arrange
        _fixture.MockRepository
            .Setup(r => r.RemoveFromRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(_fixture.SucceededResult);

        _fixture.MockRepository
            .Setup(r => r.RemoveFromRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(_fixture.FailedResult);

        // Act
        var result = async () => await _fixture.ChangeUserRoleCommandHandler
            .Handle(_fixture.ChangeUserRoleCommand, _fixture.CancellationToken);

        // Assert
        await result.Should().ThrowAsync<OperationFailedException>();
    }
}
