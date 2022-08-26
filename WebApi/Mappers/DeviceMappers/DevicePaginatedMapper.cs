using Core.Entities;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.DeviceViewModels;
using WebApi.Mappers.Interfaces;

namespace WebApi.Mappers.DeviceMappers
{
    public class DevicePaginatedMapper : IMapper<PaginatedList<Device>, PageViewModel<DeviceReadViewModel>>
    {
        private readonly IMapper<Device, DeviceReadViewModel> _readMapper;

        public DevicePaginatedMapper(IMapper<Device, DeviceReadViewModel> readMapper)
        {
            _readMapper = readMapper;
        }

        public PageViewModel<DeviceReadViewModel> Map(PaginatedList<Device> source)
        {
            return new PageViewModel<DeviceReadViewModel>()
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
