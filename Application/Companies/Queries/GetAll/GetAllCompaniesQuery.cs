using Application.Abstractions.MediatR;
using Domain.Entities;
using Domain.Shared;

namespace Application.Companies.Queries.GetAll;

public sealed record GetAllCompaniesQuery(
    int PageNumber,
    int PageSize) : IQuery<PaginatedList<Company>>;
