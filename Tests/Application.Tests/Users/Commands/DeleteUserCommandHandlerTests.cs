using Application.Tests.Users.Commands.Fixtures;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Application.Tests.Users.Commands;

public class DeleteUserCommandHandlerTests : IClassFixture<DeleteUserCommandHandlerFixture>
{
    private readonly DeleteUserCommandHandlerFixture _fixture;

	public DeleteUserCommandHandlerTests(DeleteUserCommandHandlerFixture fixture)
	{
		_fixture = fixture;
    }

    [Fact]
    public async Task Handle_Should_ReturnUnit_WhenDeleteSucceeded()
    {
        // Arrange
        _fixture.MockRepository
            .Setup(r => r.DeleteAsync(It.IsAny<User>()))
            .ReturnsAsync(_fixture.SucceededResult);

        // Act
        var result = await _fixture.DeleteUserCommandHandler.Handle(_fixture.DeleteUserCommand, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<Unit>();
    }

    [Fact]
    public async Task Handle_Should_ThrowOperationFailedException_WhenDeleteFailed()
    {
        // Arrange
        _fixture.MockRepository
            .Setup(r => r.DeleteAsync(It.IsAny<User>()))
            .ReturnsAsync(_fixture.FailedResult);

        // Act
        var result = async () => await _fixture.DeleteUserCommandHandler
            .Handle(_fixture.DeleteUserCommand, _fixture.CancellationToken);

        // Assert
        await result.Should().ThrowAsync<OperationFailedException>();
    }
}
