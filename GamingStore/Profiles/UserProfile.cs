using GamingStore.Models;
using GamingStore.Dtos.User;
using AutoMapper;

namespace GamingStore.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserReadDto>();
            CreateMap<UserRegisterDto, User>();
            CreateMap<User, UserWithTokenReadDto>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}
