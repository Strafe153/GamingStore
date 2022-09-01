using Core.Entities;
using Core.Enums;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(User entity)
        {
            try
            {
                _repository.Create(entity);
                await _repository.SaveChangesAsync();
            } 
            catch (DbUpdateException)
            {
                throw new UsernameNotUniqueException($"Username '{entity.Username}' is already taken");
            }
        }

        public async Task DeleteAsync(User entity)
        {
            _repository.Delete(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task<PaginatedList<User>> GetAllAsync(int pageNumber, int pageSize)
        {
            var users = await _repository.GetAllAsync(pageNumber, pageSize);
            return users;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user is null)
            {
                throw new NullReferenceException($"User with id {id} not found");
            }

            return user;
        }

        public async Task<User> GetByNameAsync(string name)
        {
            var user = await _repository.GetByNameAsync(name);

            if (user is null)
            {
                throw new NullReferenceException($"User with name {name} not found");
            }

            return user;
        }

        public async Task UpdateAsync(User entity)
        {
            _repository.Update(entity);
            await _repository.SaveChangesAsync();
        }

        public User ConstructUser(string name, byte[] passwordHash, byte[] passwordSalt)
        {
            User user = new()
            {
                Username = name,
                Role = UserRole.User,
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
            if (performedOn.Username != performer.Name
                && !claims.Any(c => c.Value == UserRole.Admin.ToString()))
            {
                throw new NotEnoughRightsException("Not enough rights to perform the operation");
            }
        }
    }
}
