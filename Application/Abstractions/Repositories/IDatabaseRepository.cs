namespace Application.Abstractions.Repositories;

public interface IDatabaseRepository
{
    Task SaveChangesAsync();
}
