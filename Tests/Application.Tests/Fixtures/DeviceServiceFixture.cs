using AutoFixture.AutoMoq;
using AutoFixture;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using Moq;
using Application.Services;
using Core.Enums;
using Microsoft.Extensions.Logging;
using Core.Interfaces.Services;

namespace Application.Tests.Fixtures
{
    public class DeviceServiceFixture
    {
        public DeviceServiceFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockDeviceRepository = fixture.Freeze<Mock<IRepository<Device>>>();
            MockPictureService = fixture.Freeze<Mock<IPictureService>>();
            MockLogger = fixture.Freeze<Mock<ILogger<DeviceService>>>();

            MockDeviceService = new(
                MockDeviceRepository.Object,
                MockPictureService.Object,
                MockLogger.Object);

            Id = 1;
            Device = GetDevice();
            PaginatedList = GetPaginatedList();
        }

        public DeviceService MockDeviceService { get; }
        public Mock<IRepository<Device>> MockDeviceRepository { get; }
        public Mock<IPictureService> MockPictureService { get; }
        public Mock<ILogger<DeviceService>> MockLogger { get; }

        public int Id { get; }
        public Device Device { get; }
        public PaginatedList<Device> PaginatedList { get; }

        private Device GetDevice()
        {
            return new Device()
            {
                Id = Id,
                Name = "Name",
                Category = DeviceCategory.Mouse,
                Price = Id,
                InStock = Id,
                CompanyId = Id
            };
        }

        private List<Device> GetDevices()
        {
            return new List<Device>()
            {
                Device,
                Device
            };
        }

        private PaginatedList<Device> GetPaginatedList()
        {
            return new PaginatedList<Device>(GetDevices(), 6, 1, 5);
        }
    }
}
