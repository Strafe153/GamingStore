using Application.Abstractions.MediatR;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Companies.Commands.Create;

public sealed record CreateCompanyCommand(
	string Name,
	IFormFile? Picture) : ICommand<Company>;