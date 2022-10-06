using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using DataAccess.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateAsync(User user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        return result;
    }

    public async Task<IdentityResult> DeleteAsync(User user)
    {
        var result = await _userManager.DeleteAsync(user);
        return result;
    }

    public async Task<PaginatedList<User>> GetAllAsync(
        int pageNumber, 
        int pageSize,
        CancellationToken token = default,
        Expression<Func<User, bool>>? filter = null)
    {
        var query = filter is null
            ? _userManager.Users
            : _userManager.Users.Where(filter);

        var users = await query
            .AsNoTracking()
            .ToPaginatedList(pageNumber, pageSize, token);

        return users;
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken token = default)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id, token);
        return user;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken token = default)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email, token);
        return user;
    }

    public async Task<IdentityResult> UpdateAsync(User user)
    {
        var result = await _userManager.UpdateAsync(user);
        return result;
    }

    public async Task<IdentityResult> AssignRoleAsync(User user, string role)
    {
        var result = await _userManager.AddToRoleAsync(user, role);
        return result;
    }

    public async Task<IdentityResult> RemoveFromRolesAsync(User user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var result = await _userManager.RemoveFromRolesAsync(user, userRoles);

        return result;
    }

    public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
    {
        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        return result;
    }
}
