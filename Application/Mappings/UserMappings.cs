using Application.Users.Commands.ChangePassword;
using Application.Users.Commands.ChangeRole;
using Application.Users.Commands.Register;
using Application.Users.Commands.Update;
using Application.Users.Queries;
using Domain.Entities;
using Domain.Shared.Paging;

namespace Application.Mappings;

public static class UserMappings
{
    public static GetUserResponse ToResponse(this User user) => new(
        user.Id,
        user.FirstName,
        user.LastName,
        user.Email,
        user.PhoneNumber,
        user.ProfilePicture);

    public static PagedModel<GetUserResponse> ToPagedModel(this PagedList<User> list) =>
        new()
        {
            Entities = list.Select(ToResponse)
        };

    public static RegisterUserCommand ToRegisterCommand(this RegisterUserRequest request) => new(
        request.FirstName,
        request.LastName,
        request.Email,
        request.Email,
        request.PhoneNumber,
        request.Password,
        request.ProfilePicture);

    public static UpdateUserCommand ToCommand(this UpdateUserRequest request) => new(
        default!,
        request.FirstName,
        request.LastName,
        request.PhoneNumber,
        request.ProfilePicture);

    public static ChangeUserPasswordCommand ToCommand(
        this ChangeUserPasswordRequest request) => new(
            default!,
            request.CurrentPassword,
            request.NewPassword);

    public static ChangeUserRoleCommand ToCommand(
        this ChangeUserRoleRequest request) => new(
            default!,
            request.Role);
}