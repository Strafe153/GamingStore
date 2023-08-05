using Application.Companies.Commands.Create;
using Application.Companies.Commands.Update;
using Application.Companies.Queries;
using Application.Companies.Queries.GetAll;
using Application.Companies.Queries.GetById;
using Domain.Entities;
using Domain.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Tests.V1.Fixtures;
using Xunit;

namespace Presentation.Tests.V1;

public class CompaniesControllerTests : IClassFixture<CompaniesControllerFixture>
{
    private readonly CompaniesControllerFixture _fixture;

    public CompaniesControllerTests(CompaniesControllerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAsync_Should_ReturnActionResultOfPaginatedModelOfGetCompanyResponse_WhenDataIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetAllCompaniesQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.PaginatedList);

        // Act
        var result = await _fixture.CompaniesController.GetAsync(_fixture.PageParameters, _fixture.CancellationToken);
        var objectResult = result.Result.As<OkObjectResult>();
        var paginatedModel = objectResult.Value.As<PaginatedModel<GetCompanyResponse>>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<PaginatedModel<GetCompanyResponse>>>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        paginatedModel.Entities.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAsync_Should_ReturnActionResultOfGetCompanyResponse_WhenCompanyExists()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetCompanyByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Company);

        // Act
        var result = await _fixture.CompaniesController.GetAsync(_fixture.Id, _fixture.CancellationToken);
        var objectResult = result.Result.As<OkObjectResult>();
        var getCompanyResponse = objectResult.Value.As<GetCompanyResponse>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<GetCompanyResponse>>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        getCompanyResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnActionResultOfGetCompanyResponse_WhenRequestIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<CreateCompanyCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Company);

        // Act
        var result = await _fixture.CompaniesController.CreateAsync(_fixture.CreateCompanyRequest, _fixture.CancellationToken);
        var objectResult = result.Result.As<CreatedAtActionResult>();
        var getCompanyResponse = objectResult.Value.As<GetCompanyResponse>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<GetCompanyResponse>>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        getCompanyResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_ReturnNoContentResult_WhenCompanyExistsAndRequestIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetCompanyByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Company);

        // Act
        var result = await _fixture.CompaniesController
            .UpdateAsync(_fixture.Id, _fixture.UpdateCompanyRequest, _fixture.CancellationToken);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task DeleteAsync_Should_ReturnNoContentResult_WhenCompanyExists()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetCompanyByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Company);

        // Act
        var result = await _fixture.CompaniesController.DeleteAsync(_fixture.Id, _fixture.CancellationToken);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
}