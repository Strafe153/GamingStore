using Domain.Entities;
using Infrastructure.Configurations.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public static class DataSeeder
{
    public static void SeedRoles(this ModelBuilder modelBuilder)
    {
        const string adminRole = "Admin";
        const string userRole = "User";

        modelBuilder.Entity<IdentityRole<int>>().HasData(new[]
        {
            new IdentityRole<int>
            { 
                Id = 1,
                Name = adminRole, 
                NormalizedName = adminRole.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new IdentityRole<int> 
            { 
                Id = 2, 
                Name = userRole, 
                NormalizedName = userRole.ToUpper()
            }
        });
    }

    public static void SeedAdmin(this ModelBuilder modelBuilder, AdminOptions adminData)
    {
        var passwordHasher = new PasswordHasher<User>();
        var admin = new User(adminData.Name, adminData.Name, adminData.Email, adminData.Email, adminData.PhoneNumber, null)
        {
            Id = 1,
            NormalizedEmail = adminData.Email.ToUpper(),
            NormalizedUserName = adminData.Email.ToUpper(),
            SecurityStamp = Guid.NewGuid().ToString()
        };

        admin.PasswordHash = passwordHasher.HashPassword(admin, adminData.Password);

        modelBuilder.Entity<User>()
            .HasData(admin);

        modelBuilder.Entity<IdentityUserRole<int>>()
            .HasData(new IdentityUserRole<int>
            {
                RoleId = 1,
                UserId = 1
            });
    }
}
