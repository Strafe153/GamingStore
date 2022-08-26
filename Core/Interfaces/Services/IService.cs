using Core.Models;
using System.Linq.Expressions;

namespace Core.Interfaces.Services
{
    public interface IService<T>
    {
        Task<PaginatedList<T>> GetAllAsync(int pageNumber, int pageSize);
        Task<T> GetByIdAsync(int id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
