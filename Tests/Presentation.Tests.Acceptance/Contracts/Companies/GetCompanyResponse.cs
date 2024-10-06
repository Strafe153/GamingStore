namespace Presentation.Tests.Acceptance.Contracts.Companies;

public sealed record GetCompanyResponse(
    int Id,
    string Name,
    string? Picture,
    ICollection<Device> Devices);