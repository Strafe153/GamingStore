using Application.Abstractions.MediatR;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Companies.Commands.Create;


public sealed record CreateCompanyCommand : ICommand<Company>
{
    public string Name { get; init; } = default!;
    public IFormFile? Picture { get; init; }
}