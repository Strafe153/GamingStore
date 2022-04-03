using GamingStore.Models;
using GamingStore.Dtos.Device;
using AutoMapper;

namespace GamingStore.Profiles
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
