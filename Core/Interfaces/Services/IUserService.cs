using Core.Entities;
using System.Security.Claims;
using System.Security.Principal;

namespace Core.Interfaces.Services
{
    public interface IUserService : IService<User>
    {
        Task<User> GetByNameAsync(string name);
        User ConstructUser(string name, byte[] passwordHash, byte[] passwordSalt);
        void ChangePasswordData(User user, byte[] passwordHash, byte[] passwordSalt);
        void VerifyUserAccessRights(User performedOn, IIdentity performer, IEnumerable<Claim> claims);
    }
}
