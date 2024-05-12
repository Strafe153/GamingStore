using Domain.Shared.Paging;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Extensions;

public static class IQueryableExtensions
{
	public static async Task<PagedList<T>> ToPagedList<T>(
		this IQueryable<T> query,
		int pageNumber,
		int pageSize,
		CancellationToken token = default)
	{
		int count = query.Count();
		var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(token);

		return new PagedList<T>(items, count, pageNumber, pageSize);
	}
}
