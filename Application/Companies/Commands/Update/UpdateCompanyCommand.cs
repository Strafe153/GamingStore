using Application.Abstractions.MediatR;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Companies.Commands.Update;

public sealed record UpdateCompanyCommand(
	Company Company,
	string Name,
	IFormFile? Picture) : ICommand;