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
            .ForMember(pd => pd.Entities, opt => opt.MapFrom(pl => pl));

        CreateMap<User, UserReadDto>();

        CreateMap<UserRegisterDto, User>()
            .ForMember(u => u.UserName, opt => opt.MapFrom(ud => ud.Email));

        CreateMap<UserBaseDto, User>();
        CreateMap<UserUpdateDto, User>();
    }
}
