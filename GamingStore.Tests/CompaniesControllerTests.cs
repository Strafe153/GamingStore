using Microsoft.AspNetCore.Mvc;
using GamingStore.Models;
using GamingStore.Controllers;
using GamingStore.Dtos.Company;
using GamingStore.Repositories.Interfaces;
using Moq;
using Xunit;
using AutoMapper;

namespace GamingStore.Tests
{
    public class CompaniesControllerTests
    {
        private static readonly Mock<IControllable<Company>> _repo = new();
        private static readonly Mock<IMapper> _mapper = new();
        private static readonly CompaniesController _controller = new(_repo.Object, _mapper.Object);

        [Fact]
        public async Task GetAllCompaniesAsync_ValidData_ReturnsOkObjectResult()
        {
            // Act
            var result = await _controller.GetAllCompaniesAsync();
            
            // Assert
            Assert.IsType<ActionResult<IEnumerable<CompanyReadDto>>>(result);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetCompanyAsync_ExistingCompany_ReturnsOkObjectResult()
        {
            // Arrange
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Company());

            // Act
            var result = await _controller.GetCompanyAsync(Guid.Empty);

            // Assert
            Assert.IsType<ActionResult<CompanyReadDto>>(result);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetCompanyAsync_NonexistingCompany_ReturnsNotFoundObjectResult()
        {
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Company?)null);

            // Act
            var result = await _controller.GetCompanyAsync(Guid.Empty);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateCompanyAsync_ValidData_ReturnsCreatedAtActionResult()
        {
            // Arrange
            _mapper.Setup(m => m.Map<CompanyReadDto>(It.IsAny<Company>())).Returns(new CompanyReadDto());

            // Act
            var result = await _controller.CreateCompanyAsync(new CompanyCreateUpdateDto());

            // Assert
            Assert.IsType<ActionResult<CompanyReadDto>>(result);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task UpdateCompanyAsync_ExistingCompany_ReturnsNoContentResult()
        {
            // Arrange
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Company());

            // Act
            var result = await _controller.UpdateCompanyAsync(Guid.Empty, new CompanyCreateUpdateDto());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateCompanyAsync_NonexistingCompany_ReturnsNotFoundObjectResult()
        {
            // Arrange
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Company?)null);

            // Act
            var result = await _controller.UpdateCompanyAsync(Guid.Empty, new CompanyCreateUpdateDto());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteCompanyAsync_ExistingCompany_ReturnsNoContentResult()
        {
            // Arrange
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Company());

            // Act
            var result = await _controller.DeleteCompanyAsync(Guid.Empty);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCompanyAsync_NonexistingCompany_ReturnsNotFoundObjectResult()
        {

            // Arrange
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Company?)null);

            // Act
            var result = await _controller.DeleteCompanyAsync(Guid.Empty);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}