namespace Infrastructure.Configurations.Models;

public class AdminOptions
{
    public const string SectionName = "Admin";

    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Password { get; set; } = default!;
}
