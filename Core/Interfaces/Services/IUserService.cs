using Core.Entities;

namespace Core.Interfaces.Services;

public interface IUserService : IService<User>
{
    Task<User> GetByEmailAsync(string email, CancellationToken token = default);
    User ConstructUser(string username, string email, string? profilePicture, byte[] passwordHash, byte[] passwordSalt);
    void ChangePasswordData(User user, byte[] passwordHash, byte[] passwordSalt);
    void VerifyUserAccessRights(User performedOn);
}
