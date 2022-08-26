using Core.Entities;
using Core.ViewModels.UserViewModels;
using WebApi.Mappers.Interfaces;

namespace WebApi.Mappers.UserMappers
{
    public class UserReadWithTokenMapper : IMapper<User, UserWithTokenReadViewModel>
    {
        public UserWithTokenReadViewModel Map(User source)
        {
            return new UserWithTokenReadViewModel()
            {
                Id = source.Id,
                Username = source.Username,
                Role = source.Role
            };
        }
    }
}
