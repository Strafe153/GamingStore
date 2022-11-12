using Application.Abstractions.MediatR;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Users.Commands.Update;

public sealed record UpdateUserCommand : ICommand
{
    public User User { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public IFormFile? ProfilePicture { get; init; }
}