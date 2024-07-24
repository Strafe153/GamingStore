using Application.Abstractions.Repositories;
using Application.Common.Extensions;
using Domain.Entities;
using Domain.Shared.Paging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public Task<IdentityResult> CreateAsync(User user, string password) => _userManager.CreateAsync(user, password);

    public Task<IdentityResult> DeleteAsync(User user) => _userManager.DeleteAsync(user);

    public Task<PagedList<User>> GetAllAsync(
        int pageNumber, 
        int pageSize,
        CancellationToken token,
        Expression<Func<User, bool>>? filter = null)
    {
        var query = filter is null
            ? _userManager.Users
            : _userManager.Users.Where(filter);

        var usersTask = query
            .AsNoTracking()
            .ToPagedList(pageNumber, pageSize, token);

        return usersTask;
    }

    public Task<User?> GetByIdAsync(int id, CancellationToken token) =>
        _userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, token);

    public Task<IdentityResult> UpdateAsync(User user) => _userManager.UpdateAsync(user);

    public Task<IdentityResult> AssignRoleAsync(User user, string role) => _userManager.AddToRoleAsync(user, role);

    public async Task<IdentityResult> RemoveFromRolesAsync(User user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var result = await _userManager.RemoveFromRolesAsync(user, userRoles);

        return result;
    }

    public Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword) =>
        _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
}
