using Application.Abstractions.MediatR;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities;
using Domain.Shared.Paging;
using Microsoft.Extensions.Logging;

namespace Application.Companies.Queries.GetAll;

public sealed class GetAllCompaniesQueryHandler : IQueryHandler<GetAllCompaniesQuery, PagedList<Company>>
{
    private readonly IRepository<Company> _companyRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetAllCompaniesQueryHandler> _logger;

    public GetAllCompaniesQueryHandler(
        IRepository<Company> companyRepository, 
        ICacheService cacheService,
        ILogger<GetAllCompaniesQueryHandler> logger)
    {
        _companyRepository = companyRepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<PagedList<Company>> Handle(GetAllCompaniesQuery query, CancellationToken cancellationToken)
    {
        var key = $"companies:{query.PageNumber}:{query.PageSize}";
        var cachedCompanies = await _cacheService.GetAsync<List<Company>>(key, cancellationToken);
        PagedList<Company> companies;

        if (cachedCompanies is null)
        {
            companies = await _companyRepository.GetAllAsync(query.PageNumber, query.PageSize, cancellationToken);
            await _cacheService.SetAsync(key, companies, cancellationToken);
        }
        else
        {
            companies = new PagedList<Company>(cachedCompanies, cachedCompanies.Count, query.PageNumber, query.PageSize);
        }

        _logger.LogInformation("Successfully retrieved all companies");

        return companies;
    }
}
