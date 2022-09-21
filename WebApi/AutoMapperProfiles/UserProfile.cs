using AutoMapper;
using Core.Dtos;
using Core.Dtos.UserDtos;
using Core.Entities;
using Core.Models;

namespace WebApi.AutoMapperProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<PaginatedList<User>, PageDto<UserReadDto>>()
            .ForMember(pvm => pvm.Entities, opt => opt.MapFrom(pl => pl));

        CreateMap<User, UserReadDto>();
        CreateMap<User, UserWithTokenReadDto>();
        CreateMap<UserBaseDto, User>();
        CreateMap<UserUpdateDto, User>();
        CreateMap<UserChangeRoleDto, User>();
    }
}
