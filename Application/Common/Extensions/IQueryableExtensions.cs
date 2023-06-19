using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Extensions;

public static class IQueryableExtensions
{
    public static async Task<PaginatedList<T>> ToPaginatedList<T>(
        this IQueryable<T> query, 
        int pageNumber, 
        int pageSize, 
        CancellationToken token = default)
    {
        int count = query.Count();
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(token);

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}
