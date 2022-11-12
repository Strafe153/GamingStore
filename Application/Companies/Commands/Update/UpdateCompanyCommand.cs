using Application.Abstractions.MediatR;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Companies.Commands.Update;

public sealed record UpdateCompanyCommand : ICommand
{
    public Company Company { get; init; } = default!;
    public string Name { get; init; } = default!;
    public IFormFile? Picture { get; init; }
}
