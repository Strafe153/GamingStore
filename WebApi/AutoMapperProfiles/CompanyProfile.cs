using AutoMapper;
using Core.Dtos;
using Core.Dtos.CompanyDtos;
using Core.Entities;
using Core.Models;

namespace WebApi.AutoMapperProfiles;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<PaginatedList<Company>, PageDto<CompanyReadDto>>()
            .ForMember(pvm => pvm.Entities, opt => opt.MapFrom(pl => pl));

        CreateMap<Company, CompanyReadDto>();
        CreateMap<CompanyBaseDto, Company>();
    }
}
