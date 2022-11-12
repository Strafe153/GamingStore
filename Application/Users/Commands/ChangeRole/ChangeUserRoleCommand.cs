using Application.Abstractions.MediatR;
using Domain.Entities;

namespace Application.Users.Commands.ChangeRole;

public sealed record ChangeUserRoleCommand : ICommand
{
    public User User { get; init; } = default!;
    public string Role { get; init; } = default!;
}
