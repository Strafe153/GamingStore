using Application.Tests.Companies.Commands.Fixtures;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Application.Tests.Companies.Commands;

public class UpdateCompanyCommandHandlerTests : IClassFixture<UpdateCompanyCommandHandlerFixture>
{
    private readonly UpdateCompanyCommandHandlerFixture _fixture;

    public UpdateCompanyCommandHandlerTests(UpdateCompanyCommandHandlerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Handle_Should_ReturnUnit_WhenNameIsUnique()
    {
        // Act
        var result = await _fixture.UpdateCompanyCommandHandler.Handle(_fixture.UpdateCompanyCommand, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<Unit>();
    }

    [Fact]
    public async Task Handle_Should_ThrowNameNotUniqueException_WhenNameIsNotUnique()
    {
        // Arrange
        _fixture.MockRepository
            .Setup(r => r.Update(It.IsAny<Company>()))
            .Throws<DbUpdateException>();

        // Act
        var result = async () => await _fixture.UpdateCompanyCommandHandler
            .Handle(_fixture.UpdateCompanyCommand, _fixture.CancellationToken);

        // Assert
        await result.Should().ThrowAsync<ValueNotUniqueException>();
    }
}
