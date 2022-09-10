using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Core.Dtos;
using Core.Dtos.CompanyDtos;
using Core.Dtos.DeviceDtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Services;
using Core.Models;
using Moq;
using WebApi.Controllers;

namespace WebApi.Tests.Fixtures
{
    public class CompaniesControllerFixture
    {
        public CompaniesControllerFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockCompanyService = fixture.Freeze<Mock<IService<Company>>>();
            MockPictureService = fixture.Freeze<Mock<IPictureService>>();
            MockMapper = fixture.Freeze<Mock<IMapper>>();

            MockCompaniesController = new(
                MockCompanyService.Object,
                MockPictureService.Object,
                MockMapper.Object);

            Id = 1;
            Name = "Company";
            Company = GetCompany();
            CompanyReadDto = GetCompanyReadDto();
            CompanyBaseDto = GetCompanyBaseDto();
            PageParameters = GetPageParameters();
            CompanyPaginatedList = GetCompanyPaginatedList();
            DevicePaginatedList = GetDevicePaginatedList();
            CompanyPageDto = GetCompanyPageDto();
            DevicePageDto = GetDevicePageDto();
        }

        public CompaniesController MockCompaniesController { get; }
        public Mock<IService<Company>> MockCompanyService { get; }
        public Mock<IPictureService> MockPictureService { get; }
        public Mock<IMapper> MockMapper { get; }

        public int Id { get; }
        public string Name { get; }
        public Company Company { get; }
        public CompanyReadDto CompanyReadDto { get; }
        public CompanyBaseDto CompanyBaseDto { get; }
        public PageParameters PageParameters { get; }
        public PaginatedList<Company> CompanyPaginatedList { get; }
        public PaginatedList<Device> DevicePaginatedList { get; }
        public PageDto<CompanyReadDto> CompanyPageDto { get; }
        public PageDto<DeviceReadDto> DevicePageDto { get; }

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

        private CompanyReadDto GetCompanyReadDto()
        {
            return new CompanyReadDto()
            {
                Id = Id,
                Name = Name
            };
        }

        private CompanyBaseDto GetCompanyBaseDto()
        {
            return new CompanyBaseDto()
            {
                Name = Name
            };
        }

        private List<CompanyReadDto> GetCompanyReadViewModels()
        {
            return new List<CompanyReadDto>()
            {
                CompanyReadDto,
                CompanyReadDto
            };
        }

        private PageDto<CompanyReadDto> GetCompanyPageDto()
        {
            return new PageDto<CompanyReadDto>()
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

        private DeviceReadDto GetDeviceReadViewModel()
        {
            return new DeviceReadDto()
            {
                Id = Id,
                Name = Name,
                Category = DeviceCategory.Mouse,
                InStock = 500,
                Price = 49.99M,
                CompanyId = Id
            };
        }

        private List<DeviceReadDto> GetDeviceReadViewModels()
        {
            return new List<DeviceReadDto>()
            {
                GetDeviceReadViewModel(),
                GetDeviceReadViewModel()
            };
        }

        private PageDto<DeviceReadDto> GetDevicePageDto()
        {
            return new PageDto<DeviceReadDto>()
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
