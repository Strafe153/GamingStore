﻿using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using GamingDevicesStore.Data;
using GamingDevicesStore.Models;
using GamingDevicesStore.Repositories.Interfaces;

namespace GamingDevicesStore.Repositories.Implementations
{
    public class UsersRepository : IUserControllable
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public UsersRepository(DataContext context, IConfiguration configuratino)
        {
            _context = context;
            _configuration = configuratino;
        }

        public void Add(User entity)
        {
            _context.Users.Add(entity);
        }

        public void GeneratePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using (HMACSHA512 hmac = new())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public string GenerateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken token = new(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddHours(1));

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByNameAsync(string name)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == name);
        }

        public void Remove(User entity)
        {
            _context.Users.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);
        }

        public bool VerifyPasswordHash(string password, byte[] hash, byte[] salt)
        {
            using (HMACSHA512 hmac = new(salt))
            {
                byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(hash);
            }
        }
    }
}