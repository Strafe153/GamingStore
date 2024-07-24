using Domain.Shared.Paging;
using System.Linq.Expressions;

namespace Application.Abstractions.Repositories;

public interface IRepository<T>
{
	Task<PagedList<T>> GetAllAsync(
		int pageNumber,
		int pageSize,
		CancellationToken token,
		Expression<Func<T, bool>>? filter = null);

	Task<T?> GetByIdAsync(int id, CancellationToken token);
	void Create(T entity);
	void Update(T entity);
	void Delete(T entity);
}
