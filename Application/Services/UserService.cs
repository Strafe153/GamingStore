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
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository repository,
            ILogger<UserService> logger)
        {
            _repository = repository;
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
                _logger.LogWarning($"Failed to register a user. The username '{entity.Username}' is already taken");
                throw new UsernameNotUniqueException($"Username '{entity.Username}' is already taken");
            }
        }

        public async Task DeleteAsync(User entity)
        {
            _repository.Delete(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation($"Succesfully deleted a user with id {entity.Id}");
        }

        public async Task<PaginatedList<User>> GetAllAsync(int pageNumber, int pageSize)
        {
            var users = await _repository.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Successfully retrieved all users");

            return users;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user is null)
            {
                _logger.LogWarning($"Failed to retrieve a user with id {id}");
                throw new NullReferenceException($"User with id {id} not found");
            }

            _logger.LogInformation($"Successfully retrieved a user with id {id}");

            return user;
        }

        public async Task<User> GetByNameAsync(string name)
        {
            var user = await _repository.GetByNameAsync(name);

            if (user is null)
            {
                _logger.LogWarning($"Failed to retrieve a user with username {name}");
                throw new NullReferenceException($"User with username {name} not found");
            }

            _logger.LogInformation($"Successfully retrieved a user with username {name}");

            return user;
        }

        public async Task UpdateAsync(User entity)
        {
            try
            {
                _repository.Update(entity);
                await _repository.SaveChangesAsync();

                _logger.LogInformation($"Successfully updated a user with id {entity.Id}");
            }
            catch (DbUpdateException)
            {
                _logger.LogWarning($"Failed to update the user with id {entity.Id}. The username '{entity.Username}' is already taken");
                throw new UsernameNotUniqueException($"Username '{entity.Username}' is already taken");
            }
        }

        public User ConstructUser(string username, string? profilePicture, byte[] passwordHash, byte[] passwordSalt)
        {
            User user = new()
            {
                Username = username,
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
                _logger.LogWarning($"User '{performer.Name}' failed to perform an operation due to insufficient access rights");
                throw new NotEnoughRightsException("Not enough rights to perform the operation");
            }
        }
    }
}
