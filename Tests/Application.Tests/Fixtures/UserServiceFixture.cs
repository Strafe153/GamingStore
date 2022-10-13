using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace Application.Tests.Fixtures;

public class UserServiceFixture
{
    public UserServiceFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        MockUserRepository = fixture.Freeze<Mock<IUserRepository>>();
        MockCacheService = fixture.Freeze<Mock<ICacheService>>();
        MockHttpContextAccessor = fixture.Freeze<Mock<IHttpContextAccessor>>();
        MockLogger = fixture.Freeze<Mock<ILogger<UserService>>>();

        MockUserService = new UserService(
            MockUserRepository.Object,
            MockCacheService.Object,
            MockHttpContextAccessor.Object,
            MockLogger.Object);

        Id = 1;
        Name = "Name";
        Bytes = Array.Empty<byte>();
        User = GetUser();
        PaginatedList = new(GetUsers(), 6, 1, 5);
        SucceededResult = IdentityResult.Success;
        FailedResult = IdentityResult.Failed();
        HttpContextWithSufficientClaims = GetHttpContextWithSufficientClaims();
        HttpContextWithInsufficientClaims = GetHttpContextWithInsufficientClaims();
    }

    public UserService MockUserService { get; }
    public Mock<IUserRepository> MockUserRepository { get; }
    public Mock<ICacheService> MockCacheService { get; }
    public Mock<IHttpContextAccessor> MockHttpContextAccessor { get; }
    public Mock<ILogger<UserService>> MockLogger { get; }

    public int Id { get; }
    public string Name { get; }
    public byte[] Bytes { get; }
    public User User { get; }
    public PaginatedList<User> PaginatedList { get; }
    public IdentityResult SucceededResult { get; }
    public IdentityResult FailedResult { get; }
    public HttpContext HttpContextWithSufficientClaims { get; }
    public HttpContext HttpContextWithInsufficientClaims { get; }

    private User GetUser()
    {
        return new User()
        {
            Id = Id,
            UserName = Name,
            PasswordHash = Name,
        };
    }

    private List<User> GetUsers()
    {
        return new List<User>()
        {
            User,
            User
        };
    }

    private IEnumerable<Claim> GetInsufficientClaims()
    {
        return new List<Claim>()
        {
            new Claim(ClaimTypes.Name, Name!),
            new Claim(ClaimTypes.Email, Name!),
            new Claim(ClaimTypes.Role, Name!)
        };
    }

    private IEnumerable<Claim> GetSufficientClaims()
    {
        return new List<Claim>()
        {
            new Claim(ClaimTypes.Name, Name!),
            new Claim(ClaimTypes.Email, Name!),
            new Claim(ClaimTypes.Role, "Admin")
        };
    }

    private HttpContext GetHttpContextWithSufficientClaims()
    {
        var identity = new ClaimsIdentity(GetSufficientClaims());
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var context = new DefaultHttpContext()
        {
            User = claimsPrincipal
        };

        return context;
    }

    private HttpContext GetHttpContextWithInsufficientClaims()
    {
        var identity = new ClaimsIdentity(GetInsufficientClaims());
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var context = new DefaultHttpContext()
        {
            User = claimsPrincipal
        };

        return context;
    }
}
