using GamingStore.Models;

namespace GamingStore.Repositories.Interfaces
{
    public interface ICompanyControllable : IControllable<Company>
    {
        Task<Company?> GetByNameAsync(string name);
    }
}
