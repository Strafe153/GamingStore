using Application.Tests.Companies.Queries.Fixtures;
using Domain.Entities;
using Domain.Shared.Paging;
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
	public async Task Handle_Should_ReturnPagedListOfCompanyFromRepository_WhenCommandIsValid()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<List<Company>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((List<Company>)null!);

        _fixture.MockRepository
			.Setup(r => r.GetAllAsync(
				It.IsAny<int>(),
				It.IsAny<int>(),
				It.IsAny<CancellationToken>(),
				It.IsAny<Expression<Func<Company, bool>>>()))
			.ReturnsAsync(_fixture.PagedList);

		// Act
		var result = await _fixture.GetAllCompaniesQueryHandler.Handle(_fixture.GetAllCompaniesQuery, _fixture.CancellationToken);

		// Assert
		result.Should().NotBeNull().And.BeOfType<PagedList<Company>>();
    }

    [Fact]
    public async Task Handle_Should_ReturnPagedListOfCompanyFromCache_WhenCommandIsValid()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<List<Company>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.PagedList);

        // Act
        var result = await _fixture.GetAllCompaniesQueryHandler.Handle(_fixture.GetAllCompaniesQuery, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<PagedList<Company>>();
    }
}
