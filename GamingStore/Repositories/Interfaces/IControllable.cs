namespace GamingStore.Repositories.Interfaces
{
    public interface IControllable<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task SaveChangesAsync();
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
