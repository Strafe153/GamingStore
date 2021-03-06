using GamingStore.Models;

namespace GamingStore.Repositories.Interfaces
{
    public interface IUserControllable : IControllable<User>
    {
        Task<List<User>> GetByNameAsync(string name);
        void GeneratePasswordHash(string password, out byte[] hash, out byte[] salt);
        bool VerifyPasswordHash(string password, byte[] hash, byte[] salt);
        string GenerateToken(User user);
        string GetDefaultPicturePath();
    }
}
