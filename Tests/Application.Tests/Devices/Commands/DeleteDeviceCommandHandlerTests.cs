using Application.Tests.Devices.Commands.Fixtures;
using FluentAssertions;
using MediatR;
using Xunit;

namespace Application.Tests.Devices.Commands;

public class DeleteDeviceCommandHandlerTests : IClassFixture<DeleteDeviceCommandHandlerFixture>
{
    private readonly DeleteDeviceCommandHandlerFixture _fixture;

	public DeleteDeviceCommandHandlerTests(DeleteDeviceCommandHandlerFixture fixture)
	{
		_fixture = fixture;
    }

    [Fact]
    public async Task Handle_Should_ReturnUnit_WhenDeviceExists()
    {
        // Act
        var result = await _fixture.DeleteDeviceCommandHandler.Handle(_fixture.DeleteDeviceCommand, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<Unit>();
    }
}
