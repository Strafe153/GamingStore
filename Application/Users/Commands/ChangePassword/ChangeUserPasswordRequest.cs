namespace Application.Users.Commands.ChangePassword;

public sealed record ChangeUserPasswordRequest(
    string CurrentPassword,
    string NewPassword);
