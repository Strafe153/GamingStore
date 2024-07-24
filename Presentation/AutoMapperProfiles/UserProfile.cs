using Application.Users.Commands.ChangePassword;
using Application.Users.Commands.ChangeRole;
using Application.Users.Commands.Register;
using Application.Users.Commands.Update;
using Application.Users.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Shared.Paging;

namespace Presentation.AutoMapperProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<PagedList<User>, PagedModel<GetUserResponse>>()
            .ForMember(m => m.Entities, o => o.MapFrom(l => l));

        CreateMap<User, GetUserResponse>();

        CreateMap<RegisterUserRequest, RegisterUserCommand>()
            .ForCtorParam(nameof(RegisterUserCommand.UserName), c => c.MapFrom(r => r.Email));

        CreateMap<UpdateUserRequest, UpdateUserCommand>()
            .ForCtorParam(nameof(UpdateUserCommand.User), c => c.MapFrom(_ => default(User)));

        CreateMap<ChangeUserPasswordRequest, ChangeUserPasswordCommand>()
            .ForCtorParam(nameof(ChangeUserPasswordCommand.User), c => c.MapFrom(_ => default(User)));

        CreateMap<ChangeUserRoleRequest, ChangeUserRoleCommand>()
            .ForCtorParam(nameof(ChangeUserRoleCommand.User), c => c.MapFrom(_ => default(User)));
    }
}
