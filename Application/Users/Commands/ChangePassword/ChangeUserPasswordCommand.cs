using Application.Abstractions.MediatR;
using Domain.Entities;

namespace Application.Users.Commands.ChangePassword;

public sealed record ChangeUserPasswordCommand(
	User User,
	string CurrentPassword,
	string NewPassword) : ICommand;