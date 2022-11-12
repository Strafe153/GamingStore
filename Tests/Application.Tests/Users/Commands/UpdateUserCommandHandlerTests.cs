using Application.Tests.Users.Commands.Fixtures;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Application.Tests.Users.Commands;

public class UpdateUserCommandHandlerTests : IClassFixture<UpdateUserCommandHandlerFixture>
{
    private readonly UpdateUserCommandHandlerFixture _fixture;

	public UpdateUserCommandHandlerTests(UpdateUserCommandHandlerFixture fixture)
	{
		_fixture = fixture;
    }

    [Fact]
    public async Task Handle_Should_ReturnUnit_WhenUpdateSucceeded()
    {
        // Arrange
        _fixture.MockRepository
            .Setup(r => r.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(_fixture.SucceededResult);

        // Act
        var result = await _fixture.UpdateUserCommandHandler.Handle(_fixture.UpdateUserCommand, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<Unit>();
    }

    [Fact]
    public async Task Handle_Should_ThrowOperationFailedException_WhenUpdateFailed()
    {
        // Arrange
        _fixture.MockRepository
            .Setup(r => r.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(_fixture.FailedResult);

        // Act
        var result = async () => await _fixture.UpdateUserCommandHandler
            .Handle(_fixture.UpdateUserCommand, _fixture.CancellationToken);

        // Assert
        await result.Should().ThrowAsync<OperationFailedException>();
    }
}
