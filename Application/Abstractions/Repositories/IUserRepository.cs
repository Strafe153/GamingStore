using Domain.Entities;
using Domain.Shared.Paging;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<PagedList<User>> GetAllAsync(
        int pageNumber,
        int pageSize,
        CancellationToken token,
        Expression<Func<User, bool>>? filter = null);

    Task<User?> GetByIdAsync(int id, CancellationToken token);
    Task<IdentityResult> CreateAsync(User user, string password);
    Task<IdentityResult> UpdateAsync(User user);
    Task<IdentityResult> DeleteAsync(User user);
    Task<IdentityResult> AssignRoleAsync(User user, string role);
    Task<IdentityResult> RemoveFromRolesAsync(User user);
    Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
}
