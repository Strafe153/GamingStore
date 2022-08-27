using AutoMapper;
using Core.Entities;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.CompanyViewModels;

namespace WebApi.AutoMapperProfiles
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<PaginatedList<Company>, PageViewModel<CompanyReadViewModel>>()
                .ForMember(pvm => pvm.Entities, opt => opt.MapFrom(pl => pl));

            CreateMap<Company, CompanyReadViewModel>();
            CreateMap<CompanyBaseViewModel, Company>();
        }
    }
}
