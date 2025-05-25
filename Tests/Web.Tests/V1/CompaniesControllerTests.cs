using Application.Companies.Commands.Create;
using Application.Companies.Queries;
using Application.Companies.Queries.GetAll;
using Application.Companies.Queries.GetById;
using Domain.Shared.Paging;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Tests.V1.Fixtures;
using Xunit;

namespace Web.Tests.V1;

public class CompaniesControllerTests : IClassFixture<CompaniesControllerFixture>
{
    private readonly CompaniesControllerFixture _fixture;

    public CompaniesControllerTests(CompaniesControllerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Get_Should_ReturnActionResultOfPagedModelOfGetCompanyResponse_WhenDataIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetAllCompaniesQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.PagedList);

        // Act
        var result = await _fixture.CompaniesController.Get(_fixture.PageParameters, _fixture.CancellationToken);
        var objectResult = result.Result.As<OkObjectResult>();
        var pagedModel = objectResult.Value.As<PagedModel<GetCompanyResponse>>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<PagedModel<GetCompanyResponse>>>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        pagedModel.Entities.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Get_Should_ReturnActionResultOfGetCompanyResponse_WhenCompanyExists()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetCompanyByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Company);

        // Act
        var result = await _fixture.CompaniesController.Get(_fixture.Id, _fixture.CancellationToken);
        var objectResult = result.Result.As<OkObjectResult>();
        var getCompanyResponse = objectResult.Value.As<GetCompanyResponse>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<GetCompanyResponse>>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        getCompanyResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_Should_ReturnActionResultOfGetCompanyResponse_WhenRequestIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<CreateCompanyCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Company);

        // Act
        var result = await _fixture.CompaniesController.Create(_fixture.CreateCompanyRequest, _fixture.CancellationToken);
        var objectResult = result.Result.As<CreatedAtActionResult>();
        var getCompanyResponse = objectResult.Value.As<GetCompanyResponse>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<ActionResult<GetCompanyResponse>>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        getCompanyResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task Update_Should_ReturnNoContentResult_WhenCompanyExistsAndRequestIsValid()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetCompanyByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Company);

        // Act
        var result = await _fixture.CompaniesController
            .Update(_fixture.Id, _fixture.UpdateCompanyRequest, _fixture.CancellationToken);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task Delete_Should_ReturnNoContentResult_WhenCompanyExists()
    {
        // Arrange
        _fixture.MockSender
            .Setup(s => s.Send(
                It.IsAny<GetCompanyByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Company);

        // Act
        var result = await _fixture.CompaniesController.Delete(_fixture.Id, _fixture.CancellationToken);
        var objectResult = result.As<NoContentResult>();

        // Assert
        result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        objectResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
}