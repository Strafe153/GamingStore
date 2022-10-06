using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public class User : IdentityUser<int>
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? ProfilePicture { get; set; }
}
