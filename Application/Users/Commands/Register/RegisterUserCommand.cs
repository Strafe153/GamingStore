using Application.Abstractions.MediatR;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Users.Commands.Register;

public sealed record RegisterUserCommand : ICommand<User>
{
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string UserName { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public string Password { get; init; } = default!;
    public IFormFile? ProfilePicture { get; init; }
}
