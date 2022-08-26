using Core.Entities;
using Core.ViewModels.CompanyViewModels;
using WebApi.Mappers.Interfaces;

namespace WebApi.Mappers.CompanyMappers
{
    public class CompanyReadMapper : IMapper<Company, CompanyReadViewModel>
    {
        public CompanyReadViewModel Map(Company source)
        {
            return new CompanyReadViewModel()
            {
                Id = source.Id,
                Name = source.Name,
                Devices = source.Devices
            };
        }
    }
}
