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
        CreateMap<PaginatedList<User>, PaginatedModel<GetUserResponse>>()
            .ForMember(pvm => pvm.Entities, opt => opt.MapFrom(pl => pl));

        CreateMap<User, GetUserResponse>();

        CreateMap<RegisterUserRequest, RegisterUserCommand>()
            .ForMember(ruc => ruc.UserName, opt => opt.MapFrom(rur => rur.Email));

        CreateMap<UpdateUserRequest, UpdateUserCommand>();
        CreateMap<ChangeUserPasswordRequest, ChangeUserPasswordCommand>();
        CreateMap<ChangeUserRoleRequest, ChangeUserRoleCommand>();
    }
}
