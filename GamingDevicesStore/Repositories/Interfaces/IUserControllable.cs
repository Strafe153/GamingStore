using GamingDevicesStore.Models;

namespace GamingDevicesStore.Repositories.Interfaces
{
    public interface IUserControllable : IControllable<User>
    {
        Task<User?> GetByNameAsync(string name);
        void GeneratePasswordHash(string password, out byte[] hash, out byte[] salt);
        bool VerifyPasswordHash(string password, byte[] hash, byte[] salt);
        string GenerateToken(User user);
    }
}
