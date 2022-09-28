using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class CompanyService : IService<Company>
{
    private readonly IRepository<Company> _repository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<CompanyService> _logger;

    public CompanyService(
        IRepository<Company> repository,
        ICacheService cacheService,
        ILogger<CompanyService> logger)
    {
        _repository = repository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task CreateAsync(Company entity)
    {
        try
        {
            _repository.Create(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Succesfully created a company");
        }
        catch (DbUpdateException)
        {
            _logger.LogWarning("Failed to create a company. The name '{Name}' is already taken", entity.Name);
            throw new UsernameNotUniqueException($"Name '{entity.Name}' is already taken");
        }
    }

    public async Task DeleteAsync(Company entity)
    {
        _repository.Delete(entity);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Succesfully deleted a company with id {Id}", entity.Id);
    }

    public async Task<PaginatedList<Company>> GetAllAsync(int pageNumber, int pageSize, CancellationToken token = default)
    {
        string key = $"companies:{pageNumber}:{pageSize}";
        var cachedCompanies = await _cacheService.GetAsync<List<Company>>(key);
        PaginatedList<Company> companies;

        if (cachedCompanies is null)
        {
            companies = await _repository.GetAllAsync(pageNumber, pageSize, token);
            await _cacheService.SetAsync(key, companies);
        }
        else
        {
            companies = new(cachedCompanies, cachedCompanies.Count, pageNumber, pageSize);
        }

        _logger.LogInformation("Successfully retrieved all companies");

        return companies;
    }

    public async Task<Company> GetByIdAsync(int id, CancellationToken token = default)
    {
        string key = $"companies:{id}";
        var company = await _cacheService.GetAsync<Company>(key);

        if (company is null)
        {
            company = await _repository.GetByIdAsync(id, token);

            if (company is null)
            {
                _logger.LogWarning("Failed to retrieve a company with id {Id}", id);
                throw new NullReferenceException($"Company with id {id} not found");
            }

            await _cacheService.SetAsync(key, company);
        }

        _logger.LogInformation("Successfully retrieved a company with id {Id}", id);

        return company;
    }

    public async Task UpdateAsync(Company entity)
    {
        try
        {
            _repository.Update(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Successfully updated a company with id {Id}", entity.Id);
        }
        catch (DbUpdateException)
        {
            _logger.LogWarning("Failed to update the company with id {Id}. The name '{Name}' is already taken", 
                entity.Id, entity.Name);
            throw new UsernameNotUniqueException($"Name '{entity.Name}' is already taken");
        }
    }
}
