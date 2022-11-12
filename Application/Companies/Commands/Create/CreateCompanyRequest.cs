using Microsoft.AspNetCore.Http;

namespace Application.Companies.Commands.Create;

public sealed record CreateCompanyRequest(
    string Name,
    IFormFile? Picture);
