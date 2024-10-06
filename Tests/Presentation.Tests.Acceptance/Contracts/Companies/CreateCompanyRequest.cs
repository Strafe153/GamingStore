using Microsoft.AspNetCore.Http;

namespace Presentation.Tests.Acceptance.Contracts.Companies;

public sealed record CreateCompanyRequest(
    string Name,
    IFormFile? Picture);
