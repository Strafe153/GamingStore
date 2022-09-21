using Core.Entities;
using Core.Enums;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Security.Principal;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository repository,
            ICacheService cacheService,
            ILogger<UserService> logger)
        {
            _repository = repository;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task CreateAsync(User entity)
        {
            try
            {
                _repository.Create(entity);
                await _repository.SaveChangesAsync();

                _logger.LogInformation("Succesfully registered a user");
            } 
            catch (DbUpdateException)
            {
                _logger.LogWarning("Failed to register a user. The email '{Email}' is already taken", entity.Email);
                throw new UsernameNotUniqueException($"Email '{entity.Email}' is already taken");
            }
        }

        public async Task DeleteAsync(User entity)
        {
            _repository.Delete(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Succesfully deleted a user with id {Id}", entity.Id);
        }

        public async Task<PaginatedList<User>> GetAllAsync(int pageNumber, int pageSize)
        {
            string key = "users";
            var cachedUsers = await _cacheService.GetAsync<List<User>>(key);
            PaginatedList<User> users;

            if (cachedUsers is null)
            {
                users = await _repository.GetAllAsync(pageNumber, pageSize);
                await _cacheService.SetAsync(key, users);
            }
            else
            {
                users = new(cachedUsers, cachedUsers.Count, pageNumber, pageSize);
            }

            _logger.LogInformation("Successfully retrieved all users");

            return users;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            string key = $"user:{id}";
            var user = await _cacheService.GetAsync<User>(key);

            if (user is null)
            {
                user = await _repository.GetByIdAsync(id);

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

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _repository.GetByEmailAsync(email);

            if (user is null)
            {
                _logger.LogWarning("Failed to retrieve a user with username {Email}", email);
                throw new NullReferenceException($"User with email {email} not found");
            }

            _logger.LogInformation("Successfully retrieved a user with username {Email}", email);

            return user;
        }

        public async Task UpdateAsync(User entity)
        {
            try
            {
                _repository.Update(entity);
                await _repository.SaveChangesAsync();

                _logger.LogInformation("Successfully updated a user with id {Id}", entity.Id);
            }
            catch (DbUpdateException)
            {
                _logger.LogWarning("Failed to update the user with id {Id}. The username '{Username}' is already taken",
                    entity.Id, entity.Username);
                throw new UsernameNotUniqueException($"Username '{entity.Username}' is already taken");
            }
        }

        public User ConstructUser(string username, string email, string? profilePicture, byte[] passwordHash, byte[] passwordSalt)
        {
            User user = new()
            {
                Username = username,
                Email = email,
                Role = UserRole.User,
                ProfilePicture = profilePicture,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            return user;
        }

        public void ChangePasswordData(User user, byte[] passwordHash, byte[] passwordSalt)
        {
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
        }

        public void VerifyUserAccessRights(User performedOn, IIdentity performer, IEnumerable<Claim> claims)
        {
            if (performedOn.Username != performer.Name && !claims.Any(c => c.Value == UserRole.Admin.ToString()))
            {
                _logger.LogWarning("User '{Name}' failed to perform an operation due to insufficient access rights", performer.Name);
                throw new NotEnoughRightsException("Not enough rights to perform the operation");
            }
        }
    }
}
