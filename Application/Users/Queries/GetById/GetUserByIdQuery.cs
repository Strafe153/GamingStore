using Application.Abstractions.MediatR;
using Domain.Entities;

namespace Application.Users.Queries.GetById;

public sealed record GetUserByIdQuery(int Id) : IQuery<User>;
