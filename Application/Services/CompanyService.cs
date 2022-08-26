using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;

namespace Application.Services
{
    public class CompanyService : IService<Company>
    {
        private readonly IRepository<Company> _repository;

        public CompanyService(IRepository<Company> repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(Company entity)
        {
            _repository.Create(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Company entity)
        {
            _repository.Delete(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task<PaginatedList<Company>> GetAllAsync(int pageNumber, int pageSize)
        {
            var companies = await _repository.GetAllAsync(pageNumber, pageSize);
            return companies;
        }

        public async Task<Company> GetByIdAsync(int id)
        {
            var company = await _repository.GetByIdAsync(id);

            if (company is null)
            {
                throw new NullReferenceException($"Company with id {id} not found");
            }

            return company;
        }

        public async Task UpdateAsync(Company entity)
        {
            _repository.Update(entity);
            await _repository.SaveChangesAsync();
        }
    }
}
