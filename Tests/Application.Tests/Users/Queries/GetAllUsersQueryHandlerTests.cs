using Application.Tests.Users.Queries.Fixtures;
using Domain.Entities;
using Domain.Shared;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Application.Tests.Users.Queries;

public class GetAllUsersQueryHandlerTests : IClassFixture<GetAllUsersQueryHandlerFixture>
{
    private readonly GetAllUsersQueryHandlerFixture _fixture;

	public GetAllUsersQueryHandlerTests(GetAllUsersQueryHandlerFixture fixture)
	{
		_fixture = fixture;
    }

    [Fact]
    public async Task Handle_Should_ReturnPaginatedListOfUserFromRepository_WhenCommandIsValid()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<List<User>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((List<User>)null!);

        _fixture.MockRepository
            .Setup(r => r.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(_fixture.PaginatedList);

        // Act
        var result = await _fixture.GetAllUsersQueryHandler.Handle(_fixture.GetAllUsersQuery, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<PaginatedList<User>>();
    }

    [Fact]
    public async Task Handle_Should_ReturnPaginatedListOfUserFromCache_WhenCommandIsValid()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<List<User>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.PaginatedList);

        // Act
        var result = await _fixture.GetAllUsersQueryHandler.Handle(_fixture.GetAllUsersQuery, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<PaginatedList<User>>();
    }
}
