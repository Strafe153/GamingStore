using GamingStore.Models;
using GamingStore.Dtos.Device;
using AutoMapper;

namespace GamingDevicesStore.Profiles
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<Device, DeviceReadDto>();
            CreateMap<DeviceCreateDto, Device>();
            CreateMap<DeviceUpdateDto, Device>();
        }
    }
}
