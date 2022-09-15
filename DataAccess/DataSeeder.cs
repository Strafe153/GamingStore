using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DataAccess
{
    public static class DataSeeder
    {
        public static void SeedAdmin(this ModelBuilder modelBuilder)
        {
            var admin = new User()
            {
                Id = 1,
                Username = "Admin",
                Email = "admin@gmail.com",
                Role = UserRole.Admin
            };

            CreatePasswordHash("qwerty", out byte[] hash, out byte[] salt);

            admin.PasswordHash = hash;
            admin.PasswordSalt = salt;

            modelBuilder
                .Entity<User>()
                .HasData(admin);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (HMACSHA512 hmac = new())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
