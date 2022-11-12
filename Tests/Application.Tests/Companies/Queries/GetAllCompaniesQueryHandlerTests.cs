using Application.Tests.Companies.Queries.Fixtures;
using Domain.Entities;
using Domain.Shared;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Application.Tests.Companies.Queries;

public class GetAllCompaniesQueryHandlerTests : IClassFixture<GetAllCompaniesQueryHandlerFixture>
{
    private readonly GetAllCompaniesQueryHandlerFixture _fixture;

	public GetAllCompaniesQueryHandlerTests(GetAllCompaniesQueryHandlerFixture fixture)
	{
		_fixture = fixture;
	}

	[Fact]
	public async Task Handle_Should_ReturnPaginatedListOfCompanyFromRepository_WhenCommandIsValid()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<List<Company>>(It.IsAny<string>()))
            .ReturnsAsync((List<Company>)null!);

        _fixture.MockRepository
			.Setup(r => r.GetAllAsync(
				It.IsAny<int>(),
				It.IsAny<int>(),
				It.IsAny<CancellationToken>(),
				It.IsAny<Expression<Func<Company, bool>>>()))
			.ReturnsAsync(_fixture.PaginatedList);

		// Act
		var result = await _fixture.GetAllCompaniesQueryHandler.Handle(_fixture.GetAllCompaniesQuery, _fixture.CancellationToken);

		// Assert
		result.Should().NotBeNull().And.BeOfType<PaginatedList<Company>>();
    }

    [Fact]
    public async Task Handle_Should_ReturnPaginatedListOfCompanyFromCache_WhenCommandIsValid()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<List<Company>>(It.IsAny<string>()))
            .ReturnsAsync(_fixture.PaginatedList);

        // Act
        var result = await _fixture.GetAllCompaniesQueryHandler.Handle(_fixture.GetAllCompaniesQuery, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<PaginatedList<Company>>();
    }
}
