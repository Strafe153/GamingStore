using GamingDevicesStore.Models;
using GamingDevicesStore.Dtos.User;
using AutoMapper;

namespace GamingDevicesStore.Profiles
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
