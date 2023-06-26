using Domain.Entities;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services;

public class ProfileService : IProfileService
{
    private readonly UserManager<User> _userManager;
    private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;

    public ProfileService(
        UserManager<User> userManager, 
        IUserClaimsPrincipalFactory<User> claimsFactory)
    {
        _userManager = userManager;
        _claimsFactory = claimsFactory;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var subjectId = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(subjectId);

        var userClaimsPrincipal = await _claimsFactory.CreateAsync(user);
        var requestedClaims = context.RequestedResources.Resources.IdentityResources.SelectMany(r => r.UserClaims);

        var claimsToAdd = userClaimsPrincipal.Claims
            .Where(c => requestedClaims.Contains(c.Type) || c.Type is "role" || c.Type is "email")
            .ToList();

        context.IssuedClaims.AddRange(claimsToAdd);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var subjectId = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(subjectId);

        context.IsActive = user is not null;
    }
}
