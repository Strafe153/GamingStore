using AutoMapper;
using Core.Dtos;
using Core.Dtos.DeviceDtos;
using Core.Entities;
using Core.Models;

namespace WebApi.AutoMapperProfiles;

public class DeviceProfile : Profile
{
    public DeviceProfile()
    {
        CreateMap<PaginatedList<Device>, PageDto<DeviceReadDto>>()
            .ForMember(rd => rd.Entities, opt => opt.MapFrom(pl => pl));

        CreateMap<Device, DeviceReadDto>();
        CreateMap<DeviceBaseDto, Device>();
    }
}
