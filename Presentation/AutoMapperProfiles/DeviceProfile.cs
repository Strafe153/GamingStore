using Application.Devices.Commands.Create;
using Application.Devices.Commands.Update;
using Application.Devices.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Shared.Paging;

namespace Presentation.AutoMapperProfiles;

public class DeviceProfile : Profile
{
    public DeviceProfile()
    {
        CreateMap<PagedList<Device>, PagedModel<GetDeviceResponse>>()
            .ForMember(pvm => pvm.Entities, opt => opt.MapFrom(pl => pl));

        CreateMap<Device, GetDeviceResponse>();
        CreateMap<CreateDeviceRequest, CreateDeviceCommand>();

        CreateMap<UpdateDeviceRequest, UpdateDeviceCommand>()
            .ForCtorParam(nameof(UpdateDeviceCommand.Device), c => c.MapFrom(_ => default(Device)));
    }
}
