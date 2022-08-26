using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Services;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.DeviceViewModels;
using Moq;
using WebApi.Controllers;
using WebApi.Mappers.Interfaces;

namespace WebApi.Tests.Fixtures
{
    public class DevicesControllerFixture
    {
        public DevicesControllerFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockDeviceService = fixture.Freeze<Mock<IDeviceService>>();
            MockDeviceReadMapper = fixture.Freeze<Mock<IMapper<Device, DeviceReadViewModel>>>();
            MockDevicePaginatedMapper = fixture.Freeze<Mock<IMapper<PaginatedList<Device>, PageViewModel<DeviceReadViewModel>>>>();
            MockDeviceCreateMapper = fixture.Freeze<Mock<IMapper<DeviceBaseViewModel, Device>>>();
            MockDeviceUpdateMapper = fixture.Freeze<Mock<IUpdateMapper<DeviceBaseViewModel, Device>>>();

            MockDevicesController = new(
                MockDeviceService.Object,
                MockDeviceReadMapper.Object,
                MockDevicePaginatedMapper.Object,
                MockDeviceCreateMapper.Object,
                MockDeviceUpdateMapper.Object);

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
        public Mock<IMapper<Device, DeviceReadViewModel>> MockDeviceReadMapper { get; }
        public Mock<IMapper<PaginatedList<Device>, PageViewModel<DeviceReadViewModel>>> MockDevicePaginatedMapper { get; }
        public Mock<IMapper<DeviceBaseViewModel, Device>> MockDeviceCreateMapper { get; }
        public Mock<IUpdateMapper<DeviceBaseViewModel, Device>> MockDeviceUpdateMapper { get; }

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
