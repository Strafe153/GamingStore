using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Services;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.DeviceViewModels;
using Moq;
using WebApi.Controllers;

namespace WebApi.Tests.Fixtures
{
    public class DevicesControllerFixture
    {
        public DevicesControllerFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockDeviceService = fixture.Freeze<Mock<IDeviceService>>();
            MockMapper = fixture.Freeze<Mock<IMapper>>();

            MockDevicesController = new(
                MockDeviceService.Object,
                MockMapper.Object);

            Id = 1;
            Name = "Device";
            Device = GetDevice();
            DeviceReadViewModel = GetDeviceReadViewModel();
            DeviceBaseViewModel = GetDeviceBaseViewModel();
            PageParameters = GetPageParameters();
            DevicePaginatedList = GetDevicePaginatedList();
            DevicePageViewModel = GetDevicePageViewModel();
        }

        public DevicesController MockDevicesController { get; }
        public Mock<IDeviceService> MockDeviceService { get; }
        public Mock<IMapper> MockMapper { get; }

        public int Id { get; }
        public string Name { get; }
        public Device Device { get; }
        public DeviceReadViewModel DeviceReadViewModel { get; }
        public DeviceBaseViewModel DeviceBaseViewModel { get; }
        public PageParameters PageParameters { get; }
        public PaginatedList<Device> DevicePaginatedList { get; }
        public PageViewModel<DeviceReadViewModel> DevicePageViewModel { get; }

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

        private PaginatedList<Device> GetDevicePaginatedList()
        {
            return new PaginatedList<Device>(GetDevices(), 6, 1, 5);
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

        private DeviceBaseViewModel GetDeviceBaseViewModel()
        {
            return new DeviceBaseViewModel()
            {
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
                DeviceReadViewModel,
                DeviceReadViewModel
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
