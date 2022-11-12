namespace Application.Users.Queries;

public sealed record GetUserResponse(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string? ProfilePicture);
