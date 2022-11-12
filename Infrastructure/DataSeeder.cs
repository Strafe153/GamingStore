using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public static class DataSeeder
{
    public static void SeedRoles(this ModelBuilder modelBuilder)
    {
        string adminRole = "Admin";
        string userRole = "User";

        modelBuilder.Entity<IdentityRole<int>>().HasData(new[]
        {
            new IdentityRole<int>() 
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
        string name = "Admin";
        string email = "admin@gmail.com";
        var passwordHasher = new PasswordHasher<User>();

        var admin = new User()
        {
            Id = 1,
            FirstName = name,
            LastName = name,
            Email = email,
            NormalizedEmail = email.ToUpper(),
            UserName = email,
            NormalizedUserName = email.ToUpper(),
            PhoneNumber = "+380990009009",
            SecurityStamp = Guid.NewGuid().ToString()
        };

        admin.PasswordHash = passwordHasher.HashPassword(admin, adminPassword);

        modelBuilder.Entity<User>()
            .HasData(admin);
        modelBuilder.Entity<IdentityUserRole<int>>()
            .HasData(new IdentityUserRole<int>()
            {
                RoleId = 1,
                UserId = 1
            });
    }
}
