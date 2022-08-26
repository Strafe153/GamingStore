using Core.Entities;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.DeviceViewModels;
using Core.ViewModels.UserViewModels;
using WebApi.Mappers.Interfaces;

namespace WebApi.Mappers.UserMappers
{
    public class UserPaginatedMapper : IMapper<PaginatedList<User>, PageViewModel<UserReadViewModel>>
    {
        private readonly IMapper<User, UserReadViewModel> _readMapper;

        public UserPaginatedMapper(IMapper<User, UserReadViewModel> readMapper)
        {
            _readMapper = readMapper;
        }

        public PageViewModel<UserReadViewModel> Map(PaginatedList<User> source)
        {
            return new PageViewModel<UserReadViewModel>()
            {
                CurrentPage = source.CurrentPage,
                TotalPages = source.TotalPages,
                PageSize = source.PageSize,
                TotalItems = source.TotalItems,
                HasPrevious = source.HasPrevious,
                HasNext = source.HasNext,
                Entities = source.Select(c => _readMapper.Map(c))
            };
        }
    }
}
