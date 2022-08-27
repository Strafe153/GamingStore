using Core.Entities;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.CompanyViewModels;
using Core.ViewModels.DeviceViewModels;
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
        public async Task GetAsync_ValidPageParameters_ReturnsActionResultOfPageViewModelOfCompanyReadViewModel()
        {
            // Arrange
            _fixture.MockCompanyService
                .Setup(s => s.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(_fixture.CompanyPaginatedList);

            _fixture.MockMapper
                .Setup(m => m.Map<PageViewModel<CompanyReadViewModel>>(It.IsAny<PaginatedList<Company>>()))
                .Returns(_fixture.CompanyPageViewModel);

            // Act
            var result = await _fixture.MockCompaniesController.GetAsync(_fixture.PageParameters);
            var pageViewModel = result.Result.As<OkObjectResult>().Value.As<PageViewModel<CompanyReadViewModel>>();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<PageViewModel<CompanyReadViewModel>>>();
            pageViewModel.Entities.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetAsync_ExistingCompany_ReturnsActionResultOfCompanyReadViewModel()
        {
            // Arrange
            _fixture.MockCompanyService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.Company);

            _fixture.MockMapper
                .Setup(m => m.Map<CompanyReadViewModel>(It.IsAny<Company>()))
                .Returns(_fixture.CompanyReadViewModel);

            // Act
            var result = await _fixture.MockCompaniesController.GetAsync(_fixture.Id);
            var readViewModel = result.Result.As<OkObjectResult>().Value.As<CompanyReadViewModel>();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<CompanyReadViewModel>>();
            readViewModel.Should().NotBeNull();
        }

        [Fact]
        public async Task GetDevicesAsync_ExistingCompany_ReturnsActionResultOfPageViewModelOfDeviceReadViewModel()
        {
            // Arrange
            _fixture.MockCompanyService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.Company);

            _fixture.MockDeviceService
                .Setup(s => s.GetByCompanyAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(_fixture.DevicePaginatedList);

            _fixture.MockMapper
                .Setup(m => m.Map<PageViewModel<DeviceReadViewModel>>(It.IsAny<PaginatedList<Device>>()))
                .Returns(_fixture.DevicePageViewModel);

            // Act
            var result = await _fixture.MockCompaniesController.GetDevicesAsync(_fixture.Id, _fixture.PageParameters);
            var pageViewModel = result.Result.As<OkObjectResult>().Value.As<PageViewModel<DeviceReadViewModel>>();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<PageViewModel<DeviceReadViewModel>>>();
            pageViewModel.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateAsync_ValidViewModel_ReturnsActionResultOfCompanyReadViewModel()
        {
            // Arrange
            _fixture.MockMapper
                .Setup(m => m.Map<Company>(It.IsAny<CompanyBaseViewModel>()))
                .Returns(_fixture.Company);

            _fixture.MockMapper
                .Setup(m => m.Map<CompanyReadViewModel>(It.IsAny<Company>()))
                .Returns(_fixture.CompanyReadViewModel);

            // Act
            var result = await _fixture.MockCompaniesController.CreateAsync(_fixture.CompanyBaseViewModel);
            var readViewModel = result.Result.As<CreatedAtActionResult>().Value.As<CompanyReadViewModel>();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<CompanyReadViewModel>>();
            readViewModel.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateAsync_ExistingCompanyValidViewModel_ReturnsNoContentResult()
        {
            // Arrange
            _fixture.MockCompanyService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.Company);

            // Act
            var result = await _fixture.MockCompaniesController.UpdateAsync(_fixture.Id, _fixture.CompanyBaseViewModel);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
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
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
