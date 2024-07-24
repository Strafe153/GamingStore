using Application.Abstractions.Repositories;
using Application.Common.Extensions;
using Domain.Entities;
using Domain.Shared.Paging;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class CompanyRepository : IRepository<Company>
{
    private readonly GamingStoreContext _context;

    public CompanyRepository(GamingStoreContext context)
    {
        _context = context;
    }

    public void Create(Company entity) => _context.Companies.Add(entity);

    public void Delete(Company entity) => _context.Companies.Remove(entity);

    public Task<PagedList<Company>> GetAllAsync(
        int pageNumber, 
        int pageSize,
        CancellationToken token,
        Expression<Func<Company, bool>>? filter = null)
    {
        var query = filter is null
            ? _context.Companies
            : _context.Companies.Where(filter);

        var companiesTask = query
            .Include(c => c.Devices)
            .AsNoTracking()
            .ToPagedList(pageNumber, pageSize, token);

        return companiesTask;
    }

    public Task<Company?> GetByIdAsync(int id, CancellationToken token) =>
        _context.Companies
            .Include(c => c.Devices)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, token);

    public void Update(Company entity) => _context.Companies.Update(entity);
}
