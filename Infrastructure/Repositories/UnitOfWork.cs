using Application.Abstractions.Repositories;

namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly GamingStoreContext _context;

    public UnitOfWork(GamingStoreContext context)
    {
        _context = context;
    }

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
