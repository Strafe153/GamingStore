using Application.Tests.Devices.Commands.Fixtures;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Application.Tests.Devices.Commands;

public class UpdateDeviceCommandHandlerTests : IClassFixture<UpdateDeviceCommandHandlerFixture>
{
    private readonly UpdateDeviceCommandHandlerFixture _fixture;

	public UpdateDeviceCommandHandlerTests(UpdateDeviceCommandHandlerFixture fixture)
	{
		_fixture = fixture;
	}

    [Fact]
    public async Task Handle_Should_ReturnUnit_WhenNameIsUnique()
    {
        // Arrange
        _fixture.MockDeviceRepository
            .Setup(r => r.Update(It.IsAny<Device>()));

        // Act
        var result = await _fixture.UpdateDeviceCommandHandler.Handle(_fixture.UpdateDeviceCommand, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<Unit>();
    }

    [Fact]
    public async Task Handle_Should_ThrowNameNotUniqueException_WhenNameIsNotUnique()
    {
        // Arrange
        _fixture.MockDeviceRepository
            .Setup(r => r.Update(It.IsAny<Device>()))
            .Throws<DbUpdateException>();

        // Act
        var result = async () => await _fixture.UpdateDeviceCommandHandler
            .Handle(_fixture.UpdateDeviceCommand, _fixture.CancellationToken);

        // Assert
        await result.Should().ThrowAsync<ValueNotUniqueException>();
    }
}
