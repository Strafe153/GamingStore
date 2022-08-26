using Core.Models;
using System.Linq.Expressions;

namespace Core.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        Task<PaginatedList<T>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? filter = null);
        Task<T?> GetByIdAsync(int id);
        Task SaveChangesAsync();
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
