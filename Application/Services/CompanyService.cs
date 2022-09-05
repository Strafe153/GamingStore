using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class CompanyService : IService<Company>
    {
        private readonly IRepository<Company> _repository;
        private readonly IPictureService _pictureService;
        private readonly ILogger<CompanyService> _logger;

        public CompanyService(
            IRepository<Company> repository,
            IPictureService pictureService,
            ILogger<CompanyService> logger)
        {
            _repository = repository;
            _pictureService = pictureService;
            _logger = logger;
        }

        public async Task CreateAsync(Company entity)
        {
            _repository.Create(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Succesfully created a company");
        }

        public async Task DeleteAsync(Company entity)
        {
            _repository.Delete(entity);
            await _repository.SaveChangesAsync();
            await _pictureService.DeleteAsync(entity.Picture!);

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
            var company = await _repository.GetByIdAsync(id);

            if (company is null)
            {
                _logger.LogWarning($"Failed to retrieve a company with id {id}");
                throw new NullReferenceException($"Company with id {id} not found");
            }

            _logger.LogInformation($"Successfully retrieved a company with id {id}");

            return company;
        }

        public async Task UpdateAsync(Company entity)
        {
            _repository.Update(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation($"Successfully updated a company with id {entity.Id}");
        }
    }
}
