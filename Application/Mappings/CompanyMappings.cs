using Application.Companies.Commands.Create;
using Application.Companies.Commands.Update;
using Application.Companies.Queries;
using Domain.Entities;
using Domain.Shared.Paging;

namespace Application.Mappings;

public static class CompanyMappings
{
    public static GetCompanyResponse ToResponse(this Company company) => new(
        company.Id,
        company.Name,
        company.Picture,
        company.Devices);

    public static PagedModel<GetCompanyResponse> ToPagedModel(this PagedList<Company> list) =>
        new()
        {
            Entities = list.Select(ToResponse)
        };

    public static CreateCompanyCommand ToCommand(this CreateCompanyRequest request) => new(
        request.Name,
        request.Picture);

    public static UpdateCompanyCommand ToCommand(this UpdateCompanyRequest request) => new(
        default!,
        request.Name,
        request.Picture
    );
}