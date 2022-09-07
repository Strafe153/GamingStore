using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Fixtures
{
    public class DeviceServiceFixture
    {
        public DeviceServiceFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockDeviceRepository = fixture.Freeze<Mock<IRepository<Device>>>();
            MockLogger = fixture.Freeze<Mock<ILogger<DeviceService>>>();

            MockDeviceService = new(
                MockDeviceRepository.Object,
                MockLogger.Object);

            Id = 1;
            Device = GetDevice();
            PaginatedList = GetPaginatedList();
        }

        public DeviceService MockDeviceService { get; }
        public Mock<IRepository<Device>> MockDeviceRepository { get; }
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
