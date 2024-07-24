using Application.Abstractions.MediatR;
using Domain.Entities;

namespace Application.Users.Commands.Delete;

public sealed record DeleteUserCommand(User User) : ICommand;