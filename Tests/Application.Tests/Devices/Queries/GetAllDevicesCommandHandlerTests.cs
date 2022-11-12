using Application.Tests.Devices.Queries.Fixtures;
using Domain.Entities;
using Domain.Shared;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Application.Tests.Devices.Queries;

public class GetAllDevicesCommandHandlerTests : IClassFixture<GetAllDevicesCommandHandlerFixture>
{
    private readonly GetAllDevicesCommandHandlerFixture _fixture;

	public GetAllDevicesCommandHandlerTests(GetAllDevicesCommandHandlerFixture fixture)
	{
		_fixture = fixture;
    }

    [Fact]
    public async Task Handle_Should_ReturnPaginatedListOfDeviceFromRepository_WhenCommandIsValid()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<List<Device>>(It.IsAny<string>()))
            .ReturnsAsync((List<Device>)null!);

        _fixture.MockRepository
            .Setup(r => r.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Device, bool>>>()))
            .ReturnsAsync(_fixture.PaginatedList);

        // Act
        var result = await _fixture.GetAllDevicesQueryHandler.Handle(_fixture.GetAllDevicesQuery, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<PaginatedList<Device>>();
    }

    [Fact]
    public async Task Handle_Should_ReturnPaginatedListOfDeviceFromCache_WhenCommandIsValid()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<List<Device>>(It.IsAny<string>()))
            .ReturnsAsync(_fixture.PaginatedList);

        // Act
        var result = await _fixture.GetAllDevicesQueryHandler.Handle(_fixture.GetAllDevicesQuery, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<PaginatedList<Device>>();
    }
}
