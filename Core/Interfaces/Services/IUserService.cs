using Core.Entities;
using System.Security.Claims;
using System.Security.Principal;

namespace Core.Interfaces.Services
{
    public interface IUserService : IService<User>
    {
        Task<User> GetByNameAsync(string name);
        Task<User> ConstructUserAsync(string username, byte[] passwordHash, byte[] passwordSalt, string? profilePicture);
        void ChangePasswordData(User user, byte[] passwordHash, byte[] passwordSalt);
        void VerifyUserAccessRights(User performedOn, IIdentity performer, IEnumerable<Claim> claims);
    }
}
