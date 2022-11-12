using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<int>
{
    public User(
        string firstName, 
        string lastName, 
        string email,
        string userName,
        string phoneNumber, 
        string? profilePicture)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserName = userName;
        PhoneNumber = phoneNumber;
        ProfilePicture = profilePicture;
    }

    public User()
    {
    }

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? ProfilePicture { get; set; }
}
