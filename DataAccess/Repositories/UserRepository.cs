using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
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
            Expression<Func<User, bool>>? filter = null)
        {
            var query = filter is null
                ? _context.Users
                : _context.Users.Where(filter);

            var users = await query.ToPaginatedList(pageNumber, pageSize);

            return users;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
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
}
