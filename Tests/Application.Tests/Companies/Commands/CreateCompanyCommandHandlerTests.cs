using Application.Tests.Companies.Commands.Fixtures;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Application.Tests.Companies.Commands;

public class CreateCompanyCommandHandlerTests : IClassFixture<CreateCompanyCommandHandlerFixture>
{
    private readonly CreateCompanyCommandHandlerFixture _fixture;

    public CreateCompanyCommandHandlerTests(CreateCompanyCommandHandlerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Handle_Should_ReturnCompany_WhenNameIsUnique()
    {
        // Arrange
        _fixture.MockRepository
            .Setup(r => r.Create(It.IsAny<Company>()));

        // Act
        var result = await _fixture.CreateCompanyCommandHandler.Handle(_fixture.CreateCompanyCommand, _fixture.CancellationToken);

        // Assert
        result.Should().NotBeNull().And.BeOfType<Company>();
    }

    [Fact]
    public async Task Handle_Should_ThrowNameNotUniqueException_WhenNameIsNotUnique()
    {
        // Arrange
        _fixture.MockRepository
            .Setup(r => r.Create(It.IsAny<Company>()))
            .Throws<DbUpdateException>();

        // Act
        var result = async () => await _fixture.CreateCompanyCommandHandler
            .Handle(_fixture.CreateCompanyCommand, _fixture.CancellationToken);

        // Assert
        await result.Should().ThrowAsync<ValueNotUniqueException>();
    }
}
