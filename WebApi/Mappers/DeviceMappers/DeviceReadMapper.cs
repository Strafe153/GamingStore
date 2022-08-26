using Core.Entities;
using Core.ViewModels.DeviceViewModels;
using WebApi.Mappers.Interfaces;

namespace WebApi.Mappers.DeviceMappers
{
    public class DeviceReadMapper : IMapper<Device, DeviceReadViewModel>
    {
        public DeviceReadViewModel Map(Device source)
        {
            return new DeviceReadViewModel()
            {
                Id = source.Id,
                Name = source.Name,
                Category = source.Category,
                Price = source.Price,
                InStock = source.InStock,
                CompanyId = source.CompanyId,
                CompanyName = source.Company!.Name
            };
        }
    }
}
