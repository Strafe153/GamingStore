using AutoMapper;
using Core.Entities;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.DeviceViewModels;

namespace WebApi.AutoMapperProfiles
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<PaginatedList<Device>, PageViewModel<DeviceReadViewModel>>()
                .ForMember(pvm => pvm.Entities, opt => opt.MapFrom(pl => pl));

            CreateMap<Device, DeviceReadViewModel>();
            CreateMap<DeviceBaseViewModel, Device>();
        }
    }
}
