using Application.Companies.Commands.Create;
using Application.Companies.Commands.Update;
using Application.Companies.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Shared.Paging;

namespace Presentation.AutoMapperProfiles;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<PagedList<Company>, PagedModel<GetCompanyResponse>>()
            .ForMember(pvm => pvm.Entities, opt => opt.MapFrom(pl => pl));

        CreateMap<Company, GetCompanyResponse>();
        CreateMap<CreateCompanyRequest, CreateCompanyCommand>();

        CreateMap<UpdateCompanyRequest, UpdateCompanyCommand>()
            .ForCtorParam(nameof(UpdateCompanyCommand.Company), c => c.MapFrom(_ => default(Company)));
    }
}
