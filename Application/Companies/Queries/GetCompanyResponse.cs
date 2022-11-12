using Domain.Entities;

namespace Application.Companies.Queries;

public sealed record GetCompanyResponse(
    int Id,
    string Name,
    string? Picture,
    ICollection<Device> Devices);
