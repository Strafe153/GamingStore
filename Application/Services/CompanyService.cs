using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
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
                _logger.LogWarning($"Failed to create a company. The name '{entity.Name}' is already taken");
                throw new UsernameNotUniqueException($"Name '{entity.Name}' is already taken");
            }
        }

        public async Task DeleteAsync(Company entity)
        {
            _repository.Delete(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation($"Succesfully deleted a company with id {entity.Id}");
        }

        public async Task<PaginatedList<Company>> GetAllAsync(int pageNumber, int pageSize)
        {
            var companies = await _repository.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Successfully retrieved all companies");

            return companies;
        }

        public async Task<Company> GetByIdAsync(int id)
        {
            string key = $"company:{id}";
            var company = await _cacheService.GetAsync<Company>(key);

            if (company is null)
            {
                company = await _repository.GetByIdAsync(id);

                if (company is null)
                {
                    _logger.LogWarning($"Failed to retrieve a company with id {id}");
                    throw new NullReferenceException($"Company with id {id} not found");
                }

                await _cacheService.SetAsync(key, company);
                _logger.LogInformation($"Successfully retrieved a company with id {id}");
            }

            return company;
        }

        public async Task UpdateAsync(Company entity)
        {
            try
            {
                _repository.Update(entity);
                await _repository.SaveChangesAsync();

                _logger.LogInformation($"Successfully updated a company with id {entity.Id}");
            }
            catch (DbUpdateException)
            {
                _logger.LogWarning($"Failed to update the company with id {entity.Id}. The name '{entity.Name}' is already taken");
                throw new UsernameNotUniqueException($"Name '{entity.Name}' is already taken");
            }
        }
    }
}
