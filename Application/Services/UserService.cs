using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly ICacheService _cacheService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository repository,
        ICacheService cacheService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<UserService> logger)
    {
        _repository = repository;
        _cacheService = cacheService;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task CreateAsync(User entity, string password)
    {
        var result = await _repository.CreateAsync(entity, password);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed to register a user. The performerEmail '{Email}' is already taken", entity.Email);
            throw new NameNotUniqueException($"Email '{entity.Email}' is already taken");
        }

        _logger.LogInformation("Successfully registered a user");
    }

    public async Task DeleteAsync(User entity)
    {
        var result = await _repository.DeleteAsync(entity);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed to delete user with username {Username}", entity.UserName);
            throw new OperationFailedException($"Failed to delete user with username {entity.UserName}");
        }

        _logger.LogInformation("Succesfully deleted user with id {Id}", entity.Id);
    }

    public async Task<PaginatedList<User>> GetAllAsync(int pageNumber, int pageSize, CancellationToken token = default)
    {
        string key = $"users:{pageNumber}:{pageSize}";
        var cachedUsers = await _cacheService.GetAsync<List<User>>(key);
        PaginatedList<User> users;

        if (cachedUsers is null)
        {
            users = await _repository.GetAllAsync(pageNumber, pageSize, token);
            await _cacheService.SetAsync(key, users);
        }
        else
        {
            users = new(cachedUsers, cachedUsers.Count, pageNumber, pageSize);
        }

        _logger.LogInformation("Successfully retrieved all users");

        return users;
    }

    public async Task<User> GetByIdAsync(int id, CancellationToken token = default)
    {
        string key = $"users:{id}";
        var user = await _cacheService.GetAsync<User>(key);

        if (user is null)
        {
            user = await _repository.GetByIdAsync(id, token);

            if (user is null)
            {
                _logger.LogWarning("Failed to retrieve a user with id {Id}", id);
                throw new NullReferenceException($"User with id {id} not found");
            }

            await _cacheService.SetAsync(key, user);
        }

        _logger.LogInformation("Successfully retrieved a user with id {Id}", id);

        return user;
    }

    public async Task<User> GetByEmailAsync(string email, CancellationToken token = default)
    {
        string key = $"users:{email}";
        var user = await _cacheService.GetAsync<User>(key);

        if (user is null)
        {
            user = await _repository.GetByEmailAsync(email, token);

            if (user is null)
            {
                _logger.LogWarning("Failed to retrieve a user with username {Email}", email);
                throw new NullReferenceException($"User with performerEmail {email} not found");
            }

            await _cacheService.SetAsync(key, user);
        }

        _logger.LogInformation("Successfully retrieved a user with username {Email}", email);

        return user;
    }

    public async Task UpdateAsync(User entity)
    {
        var result = await _repository.UpdateAsync(entity);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed to update the user with id {Id}. The username '{Username}' is already taken",
                entity.Id, entity.UserName);
            throw new NameNotUniqueException($"Username '{entity.UserName}' is already taken");
        }

        _logger.LogInformation("Successfully updater user with id {Id}", entity.Id);
    }

    public async Task AssignRoleAsync(User user, string role)
    {
        var result = await _repository.RemoveFromRolesAsync(user);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed to remove roles from user with id {Id}", user.Id);
            throw new OperationFailedException($"Failed to remove roles from user with id {user.Id}");
        }

        result = await _repository.AssignRoleAsync(user, role);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed to assign performerRole {Role} to user with id {Id}", role, user.Id);
            throw new OperationFailedException($"Failed to assign performerRole {role} to user with id {user.Id}");
        }

        _logger.LogInformation("Successfully assigned performerRole {Role} to user with id {Id}", role, user.Id);
    }

    public async Task ChangePasswordAsync(User user, string currentPassword, string newPassword)
    {
        var result = await _repository.ChangePasswordAsync(user, currentPassword, newPassword);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed to change password for user with id {Id}", user.Id);
            throw new OperationFailedException($"Failed to change password for user with id {user.Id}");
        }

        _logger.LogInformation("Successfully changed password for user with id {Id}", user.Id);
    }

    public void VerifyUserAccessRights(User performedOn)
    {
        var context = _httpContextAccessor.HttpContext;
        string performerRole = context.User.Claims.First(c => c.Type.Contains("role")).Value;
        string performerEmail = context.User.Claims.First(c => c.Type.Contains("email")).Value;

        if ((performedOn.Email != performerEmail) && (performerRole != "Admin"))
        {
            _logger.LogWarning("User '{Name}' failed to perform an operation due to insufficient access rights",
                context.User.Identity!.Name);
            throw new NotEnoughRightsException("Not enough rights to perform the operation");
        }
    }
}
