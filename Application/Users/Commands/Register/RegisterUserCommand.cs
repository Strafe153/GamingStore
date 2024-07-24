using Application.Abstractions.MediatR;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Users.Commands.Register;

public sealed record RegisterUserCommand(
	string FirstName,
	string LastName,
	string Email,
	string UserName,
	string PhoneNumber,
	string Password,
	IFormFile? ProfilePicture) : ICommand<User>;