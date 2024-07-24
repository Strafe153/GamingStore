using Application.Abstractions.MediatR;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Users.Commands.Update;

public sealed record UpdateUserCommand(
	User User,
	string FirstName,
	string LastName,
	string PhoneNumber,
	IFormFile? ProfilePicture) : ICommand;