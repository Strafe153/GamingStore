using Application.Tests.Fixtures;
using Core.Entities;
using Core.Models;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Application.Tests
{
    public class CompanyServiceTests : IClassFixture<CompanyServiceFixture>
    {
        private readonly CompanyServiceFixture _fixture;

        public CompanyServiceTests(CompanyServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllAsync_ValidParametersDataFromRepository_ReturnsPaginatedListOfCompany()
        {
            // Arrange
            _fixture.MockCacheService
                .Setup(s => s.GetAsync<List<Company>>(It.IsAny<string>()))
                .ReturnsAsync((List<Company>)null!);

            _fixture.MockCompanyRepository
                .Setup(r => r.GetAllAsync(
                    It.IsAny<int>(), 
                    It.IsAny<int>(),
                    It.IsAny<Expression<Func<Company, bool>>>()))
                .ReturnsAsync(_fixture.PaginatedList);

            // Act
            var result = await _fixture.MockCompanyService.GetAllAsync(_fixture.Id, _fixture.Id);

            // Assert
            result.Should().NotBeNull().And.NotBeEmpty().And.BeOfType<PaginatedList<Company>>();
        }

        [Fact]
        public async Task GetAllAsync_ValidParametersDataFromCache_ReturnsPaginatedListOfCompany()
        {
            // Arrange
            _fixture.MockCacheService
                .Setup(s => s.GetAsync<List<Company>>(It.IsAny<string>()))
                .ReturnsAsync(_fixture.PaginatedList);

            // Act
            var result = await _fixture.MockCompanyService.GetAllAsync(_fixture.Id, _fixture.Id);

            // Assert
            result.Should().NotBeNull().And.NotBeEmpty().And.BeOfType<PaginatedList<Company>>();
        }

        [Fact]
        public async Task GetByIdAsync_ExistingCompanyInRepository_ReturnsCompany()
        {
            // Arrange
            _fixture.MockCacheService
                .Setup(s => s.GetAsync<Company>(It.IsAny<string>()))
                .ReturnsAsync((Company)null!);

            _fixture.MockCompanyRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.Company);

            // Act
            var result = await _fixture.MockCompanyService.GetByIdAsync(_fixture.Id);

            // Assert
            result.Should().NotBeNull().And.BeOfType<Company>();
        }

        [Fact]
        public async Task GetByIdAsync_ExistingCompanyInCache_ReturnsCompany()
        {
            // Arrange
            _fixture.MockCacheService
                .Setup(s => s.GetAsync<Company>(It.IsAny<string>()))
                .ReturnsAsync(_fixture.Company);

            // Act
            var result = await _fixture.MockCompanyService.GetByIdAsync(_fixture.Id);

            // Assert
            result.Should().NotBeNull().And.BeOfType<Company>();
        }

        [Fact]
        public async Task GetByIdAsync_NonexistingCompany_ThrowsNullReferenceException()
        {
            // Arrange
            _fixture.MockCacheService
                .Setup(s => s.GetAsync<Company>(It.IsAny<string>()))
                .ReturnsAsync((Company)null!);

            _fixture.MockCompanyRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Company)null!);

            // Act
            var result = async () => await _fixture.MockCompanyService.GetByIdAsync(_fixture.Id);

            // Assert
            await result.Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public void CreateAsync_ValidCompany_ReturnsTask()
        {
            // Act
            var result = _fixture.MockCompanyService.CreateAsync(_fixture.Company);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void UpdateAsync_ValidCompany_ReturnsTask()
        {
            // Act
            var result = _fixture.MockCompanyService.UpdateAsync(_fixture.Company);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void DeleteAsync_ValidCompany_ReturnsTask()
        {
            // Act
            var result = _fixture.MockCompanyService.DeleteAsync(_fixture.Company);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
