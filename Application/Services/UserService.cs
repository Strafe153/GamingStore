using Application.Abstractions.Services;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IHttpContextAccessor httpContextAccessor,
        ILogger<UserService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public void VerifyUserAccessRights(User performedOn)
    {
        var context = _httpContextAccessor.HttpContext;
        string performerRole = context.User.Claims.First(c => c.Type.Contains("role")).Value;
        string performerEmail = context.User.Claims.First(c => c.Type.Contains("email")).Value;

        if ((performedOn.Email != performerEmail) && (performerRole != "Admin"))
        {
            _logger.LogWarning("User '{Name}' failed to perform an operation due to insufficient access rights",
                context.User.Identity!.Name);
            throw new NotEnoughRightsException("Not enough rights to perform the operation");
        }
    }
}
