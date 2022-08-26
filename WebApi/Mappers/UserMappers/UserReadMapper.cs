using Core.Entities;
using Core.ViewModels.UserViewModels;
using WebApi.Mappers.Interfaces;

namespace WebApi.Mappers.UserMappers
{
    public class UserReadMapper : IMapper<User, UserReadViewModel>
    {
        public UserReadViewModel Map(User source)
        {
            return new UserReadViewModel()
            {
                Id = source.Id,
                Username = source.Username,
                Role = source.Role
            };
        }
    }
}
