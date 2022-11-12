using Microsoft.AspNetCore.Http;

namespace Application.Companies.Commands.Update;

public sealed record UpdateCompanyRequest(
    string Name,
    IFormFile? Picture);
