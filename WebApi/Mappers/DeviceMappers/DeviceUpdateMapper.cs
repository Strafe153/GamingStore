using Core.Entities;
using Core.ViewModels.DeviceViewModels;
using WebApi.Mappers.Interfaces;

namespace WebApi.Mappers.DeviceMappers
{
    public class DeviceUpdateMapper : IUpdateMapper<DeviceBaseViewModel, Device>
    {
        public void Map(DeviceBaseViewModel source, Device destination)
        {
            destination.Name = source.Name;
            destination.Category = source.Category;
            destination.Price = source.Price;
            destination.InStock = source.InStock;
            destination.CompanyId = source.CompanyId;
        }
    }
}
