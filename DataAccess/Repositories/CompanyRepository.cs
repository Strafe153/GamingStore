using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories;

public class CompanyRepository : IRepository<Company>
{
    private readonly GamingStoreContext _context;

    public CompanyRepository(GamingStoreContext context)
    {
        _context = context;
    }

    public void Create(Company entity)
    {
        _context.Companies.Add(entity);
    }

    public void Delete(Company entity)
    {
        _context.Companies.Remove(entity);
    }

    public async Task<PaginatedList<Company>> GetAllAsync(
        int pageNumber, 
        int pageSize,
        Expression<Func<Company, bool>>? filter = null)
    {
        var query = filter is null
            ? _context.Companies
            : _context.Companies.Where(filter);

        var companies = await query
            .Include(c => c.Devices)
            .ToPaginatedList(pageNumber, pageSize);

        return companies;
    }

    public async Task<Company?> GetByIdAsync(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Devices)
            .SingleOrDefaultAsync(c => c.Id == id);

        return company;
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
