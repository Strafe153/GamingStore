using Microsoft.AspNetCore.Http;

namespace Application.Users.Commands.Register;

public sealed record RegisterUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Password,
    IFormFile? ProfilePicture);
