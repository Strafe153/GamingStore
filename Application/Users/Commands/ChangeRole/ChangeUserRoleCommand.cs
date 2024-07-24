using Application.Abstractions.MediatR;
using Domain.Entities;

namespace Application.Users.Commands.ChangeRole;

public sealed record ChangeUserRoleCommand(
	User User,
	string Role) : ICommand;