using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public static class DataSeeder
{
    public static void SeedRoles(this ModelBuilder modelBuilder)
    {
        var adminRole = "Admin";
        var userRole = "User";

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

    public static void SeedAdmin(this ModelBuilder modelBuilder, string adminPassword)
    {
        var name = "Admin";
        var email = "admin@gmail.com";
        var phoneNumber = "+380990009009";
        var passwordHasher = new PasswordHasher<User>();

        var admin = new User(name, name, email, email, phoneNumber, null)
        {
            Id = 1,
            NormalizedEmail = email.ToUpper(),
            NormalizedUserName = email.ToUpper(),
            SecurityStamp = Guid.NewGuid().ToString()
        };

        admin.PasswordHash = passwordHasher.HashPassword(admin, adminPassword);

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
