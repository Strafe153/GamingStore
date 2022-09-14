﻿using Core.Dtos;
using Core.Dtos.CompanyDtos;
using Core.Dtos.DeviceDtos;
using Core.Entities;
using Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Tests.Fixtures;
using Xunit;

namespace WebApi.Tests
{
    public class CompaniesContorllerTests : IClassFixture<CompaniesControllerFixture>
    {
        private readonly CompaniesControllerFixture _fixture;

        public CompaniesContorllerTests(CompaniesControllerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAsync_ValidPageParameters_ReturnsActionResultOfPageDtoOfCompanyReadDto()
        {
            // Arrange
            _fixture.MockCompanyService
                .Setup(s => s.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(_fixture.CompanyPaginatedList);

            _fixture.MockMapper
                .Setup(m => m.Map<PageDto<CompanyReadDto>>(It.IsAny<PaginatedList<Company>>()))
                .Returns(_fixture.CompanyPageDto);

            // Act
            var result = await _fixture.MockCompaniesController.GetAsync(_fixture.PageParameters);
            var pageDto = result.Result.As<OkObjectResult>().Value.As<PageDto<CompanyReadDto>>();

            // Assert
            result.Should().NotBeNull().And.BeOfType<ActionResult<PageDto<CompanyReadDto>>>();
            pageDto.Entities.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetAsync_ExistingCompany_ReturnsActionResultOfCompanyReadDto()
        {
            // Arrange
            _fixture.MockCompanyService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.Company);

            _fixture.MockMapper
                .Setup(m => m.Map<CompanyReadDto>(It.IsAny<Company>()))
                .Returns(_fixture.CompanyReadDto);

            // Act
            var result = await _fixture.MockCompaniesController.GetAsync(_fixture.Id);
            var readDto = result.Result.As<OkObjectResult>().Value.As<CompanyReadDto>();

            // Assert
            result.Should().NotBeNull().And.BeOfType<ActionResult<CompanyReadDto>>();
            readDto.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateAsync_ValidDto_ReturnsActionResultOfCompanyReadDto()
        {
            // Arrange
            _fixture.MockMapper
                .Setup(m => m.Map<Company>(It.IsAny<CompanyBaseDto>()))
                .Returns(_fixture.Company);

            _fixture.MockMapper
                .Setup(m => m.Map<CompanyReadDto>(It.IsAny<Company>()))
                .Returns(_fixture.CompanyReadDto);

            // Act
            var result = await _fixture.MockCompaniesController.CreateAsync(_fixture.CompanyCreateUpdateDto);
            var readDto = result.Result.As<CreatedAtActionResult>().Value.As<CompanyReadDto>();

            // Assert
            result.Should().NotBeNull().And.BeOfType<ActionResult<CompanyReadDto>>();
            readDto.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateAsync_ExistingCompanyValidDto_ReturnsNoContentResult()
        {
            // Arrange
            _fixture.MockCompanyService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.Company);

            // Act
            var result = await _fixture.MockCompaniesController.UpdateAsync(_fixture.Id, _fixture.CompanyCreateUpdateDto);

            // Assert
            result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteAsync_ExistingCompany_ReturnsNoContentResult()
        {
            // Arrange
            _fixture.MockCompanyService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.Company);

            // Act
            var result = await _fixture.MockCompaniesController.DeleteAsync(_fixture.Id);

            // Assert
            result.Should().NotBeNull().And.BeOfType<NoContentResult>();
        }
    }
}
