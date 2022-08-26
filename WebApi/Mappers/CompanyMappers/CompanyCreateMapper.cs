using Core.Entities;
using Core.ViewModels.CompanyViewModels;
using WebApi.Mappers.Interfaces;

namespace WebApi.Mappers.CompanyMappers
{
    public class CompanyCreateMapper : IMapper<CompanyBaseViewModel, Company>
    {
        public Company Map(CompanyBaseViewModel source)
        {
            return new Company()
            {
                Name = source.Name
            };
        }
    }
}
