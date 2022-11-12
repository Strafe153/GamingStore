using Domain.Entities;

namespace Application.Abstractions.Services;

public interface IUserService
{
    void VerifyUserAccessRights(User performedOn);
}
