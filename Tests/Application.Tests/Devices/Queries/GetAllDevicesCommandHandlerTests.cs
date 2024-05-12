using Application.Tests.Devices.Queries.Fixtures;
using Domain.Entities;
using Domain.Shared.Paging;
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
    public async Task Handle_Should_ReturnPagedListOfDeviceFromRepository_WhenCommandIsValid()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<List<Device>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((List<Device>)null!);

        _fixture.MockRepository
            .Setup(r => r.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Device, bool>>>()))
            .ReturnsAsync(_fixture.PagedList);

        // Act
        var result = await _fixture.GetAllDevicesQueryHandler.Handle(_fixture.GetAllDevicesQuery, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<PagedList<Device>>();
    }

    [Fact]
    public async Task Handle_Should_ReturnPagedListOfDeviceFromCache_WhenCommandIsValid()
    {
        // Arrange
        _fixture.MockCacheService
            .Setup(s => s.GetAsync<List<Device>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.PagedList);

        // Act
        var result = await _fixture.GetAllDevicesQueryHandler.Handle(_fixture.GetAllDevicesQuery, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<PagedList<Device>>();
    }
}
