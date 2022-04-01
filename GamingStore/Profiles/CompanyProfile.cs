using GamingStore.Models;
using GamingStore.Dtos.Company;
using AutoMapper;

namespace GamingDevicesStore.Profiles
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyReadDto>();
            CreateMap<CompanyCreateUpdateDto, Company>();
        }
    }
}
