using Core.Entities;
using Core.ViewModels.DeviceViewModels;
using WebApi.Mappers.Interfaces;

namespace WebApi.Mappers.DeviceMappers
{
    public class DeviceCreateUpdateMapper : IMapper<DeviceBaseViewModel, Device>
    {
        public Device Map(DeviceBaseViewModel source)
        {
            return new Device()
            {
                Name = source.Name,
                Category = source.Category,
                Price = source.Price,
                InStock = source.InStock,
                CompanyId = source.CompanyId
            };
        }
    }
}
