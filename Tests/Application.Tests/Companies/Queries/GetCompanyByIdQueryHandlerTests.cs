using Application.Tests.Companies.Queries.Fixtures;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Tests.Companies.Queries;

public class GetCompanyByIdQueryHandlerTests : IClassFixture<GetCompanyByIdQueryHandlerFixture>
{
    private readonly GetCompanyByIdQueryHandlerFixture _fixture;

	public GetCompanyByIdQueryHandlerTests(GetCompanyByIdQueryHandlerFixture fixture)
	{
		_fixture = fixture;
	}

	[Fact]
	public async Task Handle_Should_ReturnCompanyFromRepository_WhenCompanyExists()
	{
		// Arrange
		_fixture.MockCacheService
			.Setup(s => s.GetAsync<Company>(It.IsAny<string>()))
			.ReturnsAsync((Company)null!);

		_fixture.MockRepository
			.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(_fixture.Company);

		// Act
		var result = await _fixture.GetCompanyByIdQueryHandler.Handle(_fixture.GetCompanyByIdQuery, _fixture.CancellationToken);

		// Assert
		result.Should().NotBeNull().And.BeOfType<Company>();
    }

    [Fact]
    public async Task Handle_Should_ReturnCompanyFromCache_WhenCompanyExists()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<Company>(It.IsAny<string>()))
            .ReturnsAsync(_fixture.Company);

        // Act
        var result = await _fixture.GetCompanyByIdQueryHandler.Handle(_fixture.GetCompanyByIdQuery, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<Company>();
    }

    [Fact]
    public async Task Handle_Should_ThrowNullReferenceException_WhenCompanyDoesNotExist()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<Company>(It.IsAny<string>()))
            .ReturnsAsync((Company)null!);

        _fixture.MockRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Company)null!);

        // Act
        var result = async () => await _fixture.GetCompanyByIdQueryHandler
            .Handle(_fixture.GetCompanyByIdQuery, _fixture.CancellationToken);

        // Assert
        await result.Should().ThrowAsync<NullReferenceException>();
    }
}
