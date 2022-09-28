using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly GamingStoreContext _context;

    public UserRepository(GamingStoreContext context)
    {
        _context = context;
    }

    public void Create(User entity)
    {
        _context.Users.Add(entity);
    }

    public void Delete(User entity)
    {
        _context.Users.Remove(entity);
    }

    public async Task<PaginatedList<User>> GetAllAsync(
        int pageNumber, 
        int pageSize,
        CancellationToken token = default,
        Expression<Func<User, bool>>? filter = null)
    {
        var query = filter is null
            ? _context.Users
            : _context.Users.Where(filter);

        var users = await query.ToPaginatedList(pageNumber, pageSize, token);

        return users;
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken token = default)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id, token);
        return user;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken token = default)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email, token);
        return user;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Update(User entity)
    {
        _context.Users.Update(entity);
    }
}
