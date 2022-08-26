using AutoFixture.AutoMoq;
using AutoFixture;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using Moq;
using Application.Services;
using Core.Enums;

namespace Application.Tests.Fixtures
{
    public class DeviceServiceFixture
    {
        public DeviceServiceFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockDeviceRepository = fixture.Freeze<Mock<IRepository<Device>>>();
            MockDeviceService = new(MockDeviceRepository.Object);

            Id = 1;
            Device = GetDevice();
            PaginatedList = GetPaginatedList();
        }

        public DeviceService MockDeviceService { get; }
        public Mock<IRepository<Device>> MockDeviceRepository { get; }

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
