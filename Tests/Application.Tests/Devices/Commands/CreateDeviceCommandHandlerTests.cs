using Application.Tests.Devices.Commands.Fixtures;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Application.Tests.Devices.Commands;

public class CreateDeviceCommandHandlerTests : IClassFixture<CreateDeviceCommandHandlerFixture>
{
    private readonly CreateDeviceCommandHandlerFixture _fixture;

	public CreateDeviceCommandHandlerTests(CreateDeviceCommandHandlerFixture fixture)
	{
		_fixture = fixture;
    }

    [Fact]
    public async Task Handle_Should_ReturnDevice_WhenNameIsUnique()
    {
        // Act
        var result = await _fixture.CreateDeviceCommandHandler.Handle(_fixture.CreateDeviceCommand, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<Device>();
    }

    [Fact]
    public async Task Handle_Should_ThrowNameNotUniqueException_WhenNameIsNotUnique()
    {
        // Arrange
        _fixture.MockRepository
            .Setup(r => r.Create(It.IsAny<Device>()))
            .Throws<DbUpdateException>();

        // Act
        var result = async () => await _fixture.CreateDeviceCommandHandler
            .Handle(_fixture.CreateDeviceCommand, _fixture.CancellationToken);

        // Assert
        await result.Should().ThrowAsync<ValueNotUniqueException>();
    }
}
