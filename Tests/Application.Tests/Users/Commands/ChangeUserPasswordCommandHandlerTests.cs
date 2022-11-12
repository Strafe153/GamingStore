using Application.Tests.Users.Commands.Fixtures;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Application.Tests.Users.Commands;

public class ChangeUserPasswordCommandHandlerTests : IClassFixture<ChangeUserPasswordCommandHandlerFixture>
{
    private readonly ChangeUserPasswordCommandHandlerFixture _fixture;

    public ChangeUserPasswordCommandHandlerTests(ChangeUserPasswordCommandHandlerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Handle_Should_ReturnUnit_WhenChangePasswordSucceeded()
    {
        // Arrange
        _fixture.MockRepository
            .Setup(r => r.ChangePasswordAsync(
                It.IsAny<User>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(_fixture.SucceededResult);

        // Act
        var result = await _fixture.ChangeUserPasswordCommandHandler
            .Handle(_fixture.ChangeUserPasswordCommand, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<Unit>();
    }

    [Fact]
    public async Task Handle_Should_ThrowOperationFailedException_WhenChangePasswordFailed()
    {
        // Arrange
        _fixture.MockRepository
            .Setup(r => r.ChangePasswordAsync(
                It.IsAny<User>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(_fixture.FailedResult);

        // Act
        var result = async () => await _fixture.ChangeUserPasswordCommandHandler
            .Handle(_fixture.ChangeUserPasswordCommand, _fixture.CancellationToken);

        // Assert
        await result.Should().ThrowAsync<OperationFailedException>();
    }
}
