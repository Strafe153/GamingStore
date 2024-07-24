using Application.Abstractions.MediatR;
using Domain.Entities;

namespace Application.Companies.Queries.GetById;

public sealed record GetCompanyByIdQuery(int Id) : IQuery<Company>;