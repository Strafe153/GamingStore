using Application.Companies.Commands.Create;
using Application.Companies.Commands.Update;
using Application.Companies.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Shared;

namespace Presentation.AutoMapperProfiles;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<PaginatedList<Company>, PaginatedModel<GetCompanyResponse>>()
            .ForMember(pvm => pvm.Entities, opt => opt.MapFrom(pl => pl));

        CreateMap<Company, GetCompanyResponse>();
        CreateMap<CreateCompanyRequest, CreateCompanyCommand>();
        CreateMap<UpdateCompanyRequest, UpdateCompanyCommand>();
    }
}
