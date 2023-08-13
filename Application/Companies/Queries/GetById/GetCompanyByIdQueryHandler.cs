using Application.Abstractions.MediatR;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Companies.Queries.GetById;

public sealed class GetCompanyByIdQueryHandler : IQueryHandler<GetCompanyByIdQuery, Company>
{
    private readonly IRepository<Company> _companyRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetCompanyByIdQueryHandler> _logger;

    public GetCompanyByIdQueryHandler(
        IRepository<Company> companyRepository, 
        ICacheService cacheService,
        ILogger<GetCompanyByIdQueryHandler> logger)
    {
        _companyRepository = companyRepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<Company> Handle(GetCompanyByIdQuery query, CancellationToken cancellationToken)
    {
        var key = $"companies:{query.Id}";
        var company = await _cacheService.GetAsync<Company>(key, cancellationToken);

        if (company is null)
        {
            company = await _companyRepository.GetByIdAsync(query.Id, cancellationToken);

            if (company is null)
            {
                _logger.LogWarning("Failed to retrieve a company with id {Id}", query.Id);
                throw new NullReferenceException($"Company with id {query.Id} not found");
            }

            await _cacheService.SetAsync(key, company, cancellationToken);
        }

        _logger.LogInformation("Successfully retrieved a company with id {Id}", query.Id);

        return company;
    }
}
