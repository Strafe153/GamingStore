using Microsoft.EntityFrameworkCore;
using GamingStore.Data;
using GamingStore.Models;
using GamingStore.Repositories.Interfaces;

namespace GamingStore.Repositories.Implementations
{
    public class CompaniesRepository : IControllable<Company>
    {
        private readonly DataContext _context;

        public CompaniesRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(Company entity)
        {
            _context.Companies.Add(entity);
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<Company?> GetByIdAsync(Guid id)
        {
            return await _context.Companies
                .Include(c => c.Devices)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Company?> GetByNameAsync(string name)
        {
            return await _context.Companies.SingleOrDefaultAsync(c => c.Name == name);
        }

        public void Remove(Company entity)
        {
            _context.Companies.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Company entity)
        {
            _context.Companies.Update(entity);
        }
    }
}
