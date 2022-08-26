using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Services;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.CompanyViewModels;
using Core.ViewModels.DeviceViewModels;
using Moq;
using System.Drawing;
using WebApi.Controllers;
using WebApi.Mappers.Interfaces;

namespace WebApi.Tests.Fixtures
{
    public class CompaniesControllerFixture
    {
        public CompaniesControllerFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockCompanyService = fixture.Freeze<Mock<IService<Company>>>();
            MockDeviceService = fixture.Freeze<Mock<IDeviceService>>();
            MockCompanyPaginatedMapper = fixture.Freeze<Mock<IMapper<PaginatedList<Company>, PageViewModel<CompanyReadViewModel>>>>();
            MockCompanyReadMapper = fixture.Freeze<Mock<IMapper<Company, CompanyReadViewModel>>>();
            MockCompanyCreateMapper = fixture.Freeze<Mock<IMapper<CompanyBaseViewModel, Company>>>();
            MockCompanyUpdateMapper = fixture.Freeze<Mock<IUpdateMapper<CompanyBaseViewModel, Company>>>();
            MockDeviceReadEnumerableMapper = fixture.Freeze<Mock<IMapper<PaginatedList<Device>, PageViewModel<DeviceReadViewModel>>>>();

            MockCompaniesController = new(
                MockCompanyService.Object,
                MockDeviceService.Object,
                MockCompanyReadMapper.Object,
                MockCompanyPaginatedMapper.Object,
                MockCompanyCreateMapper.Object,
                MockCompanyUpdateMapper.Object,
                MockDeviceReadEnumerableMapper.Object);

            Id = 1;
            Name = "Company";
            Company = GetCompany();
            CompanyReadViewModel = GetCompanyReadViewModel();
            CompanyBaseViewModel = GetCompanyBaseViewModel();
            PageParameters = GetPageParameters();
            CompanyPaginatedList = GetCompanyPaginatedList();
            DevicePaginatedList = GetDevicePaginatedList();
            CompanyPageViewModel = GetCompanyPageViewModel();
            DevicePageViewModel = GetDevicePageViewModel();
        }

        public CompaniesController MockCompaniesController { get; }
        public Mock<IService<Company>> MockCompanyService { get; }
        public Mock<IDeviceService> MockDeviceService { get; }
        public Mock<IMapper<PaginatedList<Company>, PageViewModel<CompanyReadViewModel>>> MockCompanyPaginatedMapper { get; }
        public Mock<IMapper<Company, CompanyReadViewModel>> MockCompanyReadMapper { get; }
        public Mock<IMapper<CompanyBaseViewModel, Company>> MockCompanyCreateMapper { get; }
        public Mock<IUpdateMapper<CompanyBaseViewModel, Company>> MockCompanyUpdateMapper { get; }
        public Mock<IMapper<PaginatedList<Device>, PageViewModel<DeviceReadViewModel>>> MockDeviceReadEnumerableMapper { get; }

        public int Id { get; }
        public string Name { get; }
        public Company Company { get; }
        public CompanyReadViewModel CompanyReadViewModel { get; }
        public CompanyBaseViewModel CompanyBaseViewModel { get; }
        public PageParameters PageParameters { get; }
        public PaginatedList<Company> CompanyPaginatedList { get; }
        public PaginatedList<Device> DevicePaginatedList { get; }
        public PageViewModel<CompanyReadViewModel> CompanyPageViewModel { get; }
        public PageViewModel<DeviceReadViewModel> DevicePageViewModel { get; }

        private Company GetCompany()
        {
            return new Company()
            {
                Id = Id,
                Name = Name
            };
        }

        private List<Company> GetCompanies()
        {
            return new List<Company>()
            {
                Company,
                Company
            };
        }

        private Device GetDevice()
        {
            return new Device()
            {
                Id = Id,
                Name = Name,
                Category = DeviceCategory.Mouse,
                InStock = 500,
                Price = 49.99M,
                CompanyId = Id
            };
        }

        private List<Device> GetDevices()
        {
            return new List<Device>()
            {
                GetDevice(),
                GetDevice()
            };
        }

        private PageParameters GetPageParameters()
        {
            return new PageParameters()
            {
                PageNumber = 1,
                PageSize = 5
            };
        }

        private PaginatedList<Company> GetCompanyPaginatedList()
        {
            return new PaginatedList<Company>(GetCompanies(), 6, 1, 5);
        }

        private PaginatedList<Device> GetDevicePaginatedList()
        {
            return new PaginatedList<Device>(GetDevices(), 6, 1, 5);
        }

        private CompanyReadViewModel GetCompanyReadViewModel()
        {
            return new CompanyReadViewModel()
            {
                Id = Id,
                Name = Name
            };
        }

        private CompanyBaseViewModel GetCompanyBaseViewModel()
        {
            return new CompanyBaseViewModel()
            {
                Name = Name
            };
        }

        private List<CompanyReadViewModel> GetCompanyReadViewModels()
        {
            return new List<CompanyReadViewModel>()
            {
                CompanyReadViewModel,
                CompanyReadViewModel
            };
        }

        private PageViewModel<CompanyReadViewModel> GetCompanyPageViewModel()
        {
            return new PageViewModel<CompanyReadViewModel>()
            {
                CurrentPage = 1,
                TotalPages = 2,
                PageSize = 5,
                TotalItems = 6,
                HasPrevious = false,
                HasNext = true,
                Entities = GetCompanyReadViewModels()
            };
        }

        private DeviceReadViewModel GetDeviceReadViewModel()
        {
            return new DeviceReadViewModel()
            {
                Id = Id,
                Name = Name,
                Category = DeviceCategory.Mouse,
                InStock = 500,
                Price = 49.99M,
                CompanyId = Id
            };
        }

        private List<DeviceReadViewModel> GetDeviceReadViewModels()
        {
            return new List<DeviceReadViewModel>()
            {
                GetDeviceReadViewModel(),
                GetDeviceReadViewModel()
            };
        }

        private PageViewModel<DeviceReadViewModel> GetDevicePageViewModel()
        {
            return new PageViewModel<DeviceReadViewModel>()
            {
                CurrentPage = 1,
                TotalPages = 2,
                PageSize = 5,
                TotalItems = 6,
                HasPrevious = false,
                HasNext = true,
                Entities = GetDeviceReadViewModels()
            };
        }
    }
}
