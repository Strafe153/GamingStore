using Microsoft.AspNetCore.Http;

namespace Application.Users.Commands.Update;

public sealed record UpdateUserRequest(
    string FirstName,
    string LastName,
    string PhoneNumber,
    IFormFile? ProfilePicture);
