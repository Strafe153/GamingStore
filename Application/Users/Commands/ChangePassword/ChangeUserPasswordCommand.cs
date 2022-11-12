using Application.Abstractions.MediatR;
using Domain.Entities;

namespace Application.Users.Commands.ChangePassword;

public sealed record ChangeUserPasswordCommand : ICommand
{
    public User User { get; init; } = default!;
    public string CurrentPassword { get; init; } = default!;
    public string NewPassword { get; init; } = default!;
}