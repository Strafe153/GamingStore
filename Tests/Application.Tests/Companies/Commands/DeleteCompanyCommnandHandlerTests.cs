using Application.Tests.Companies.Commands.Fixtures;
using FluentAssertions;
using MediatR;
using Xunit;

namespace Application.Tests.Companies.Commands;

public class DeleteCompanyCommnandHandlerTests : IClassFixture<DeleteCompanyCommandHandlerFixture>
{
    private readonly DeleteCompanyCommandHandlerFixture _fixture;

    public DeleteCompanyCommnandHandlerTests(DeleteCompanyCommandHandlerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Handle_Should_ReturnUnit_WhenCompanyExists()
    {
        // Act
        var result = await _fixture.DeleteCompanyCommandHandler.Handle(_fixture.DeleteCompanyCommand, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<Unit>();
    }
}
