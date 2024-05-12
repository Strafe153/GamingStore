using Application.Abstractions.MediatR;
using Domain.Entities;
using Domain.Shared.Paging;

namespace Application.Users.Queries.GetAll;

public sealed record GetAllUsersQuery(
    int PageNumber, 
    int PageSize) : IQuery<PagedList<User>>;
