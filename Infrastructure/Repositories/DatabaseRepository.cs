using Application.Abstractions.Repositories;

namespace Infrastructure.Repositories;

public class DatabaseRepository : IDatabaseRepository
{
	private readonly GamingStoreContext _context;

	public DatabaseRepository(GamingStoreContext context)
	{
		_context = context;
	}

	public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
