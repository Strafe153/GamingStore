using Core.Entities;
using Core.ViewModels.CompanyViewModels;
using WebApi.Mappers.Interfaces;

namespace WebApi.Mappers.CompanyMappers
{
    public class CompanyUpdateMapper : IUpdateMapper<CompanyBaseViewModel, Company>
    {
        public void Map(CompanyBaseViewModel source, Company destination)
        {
            destination.Name = source.Name;
        }
    }
}
