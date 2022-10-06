using Core.Entities;
using Core.Models;

namespace Core.Interfaces.Services;

public interface IUserService
{
    Task<PaginatedList<User>> GetAllAsync(int pageNumber, int pageSize, CancellationToken token = default);
    Task<User> GetByIdAsync(int id, CancellationToken token = default);
    Task<User> GetByEmailAsync(string email, CancellationToken token = default);
    Task CreateAsync(User user, string password);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task AssignRoleAsync(User user, string role);
    Task ChangePasswordAsync(User user, string currentPassword, string newPassword);
    void VerifyUserAccessRights(User performedOn);
}
