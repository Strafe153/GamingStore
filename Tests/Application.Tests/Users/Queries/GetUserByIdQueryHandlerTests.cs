using Application.Tests.Users.Queries.Fixtures;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Tests.Users.Queries;

public class GetUserByIdQueryHandlerTests : IClassFixture<GetUserByIdQueryHandlerFixture>
{
    private readonly GetUserByIdQueryHandlerFixture _fixture;

	public GetUserByIdQueryHandlerTests(GetUserByIdQueryHandlerFixture fixture)
	{
		_fixture = fixture;
	}

    [Fact]
    public async Task Handle_Should_ReturnUserFromRepository_WhenUserExists()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<User>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        _fixture.MockRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.User);

        // Act
        var result = await _fixture.GetUserByIdQueryHandler.Handle(_fixture.GetUserByIdQuery, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<User>();
    }

    [Fact]
    public async Task Handle_Should_ReturnUserFromCache_WhenUserExists()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<User>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.User);

        // Act
        var result = await _fixture.GetUserByIdQueryHandler.Handle(_fixture.GetUserByIdQuery, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<User>();
    }

    [Fact]
    public async Task Handle_Should_ThrowNullReferenceException_WhenUserDoesNotExist()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<User>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        _fixture.MockRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        // Act
        var result = async () => await _fixture.GetUserByIdQueryHandler
            .Handle(_fixture.GetUserByIdQuery, _fixture.CancellationToken);

        // Assert
        await result.Should().ThrowAsync<NullReferenceException>();
    }
}
