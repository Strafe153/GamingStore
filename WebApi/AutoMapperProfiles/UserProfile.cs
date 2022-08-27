using AutoMapper;
using Core.Entities;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.UserViewModels;

namespace WebApi.AutoMapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<PaginatedList<User>, PageViewModel<UserReadViewModel>>()
                .ForMember(pvm => pvm.Entities, opt => opt.MapFrom(pl => pl));

            CreateMap<User, UserReadViewModel>();
            CreateMap<User, UserWithTokenReadViewModel>();
            CreateMap<UserBaseViewModel, User>();
            CreateMap<UserChangeRoleViewModel, User>();
        }
    }
}
