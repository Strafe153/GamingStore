using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace Application.Tests.Services.Fixtures;

public class UserServiceFixture
{
	public UserServiceFixture()
	{
		var fixture = new Fixture().Customize(new AutoMoqCustomization());

		MockHttpContextAccessor = fixture.Freeze<Mock<IHttpContextAccessor>>();
		MockLogger = fixture.Freeze<Mock<ILogger<UserService>>>();

		UserService = new UserService(
			MockHttpContextAccessor.Object,
			MockLogger.Object);

        Name = "Name";
        User = GetUser();
        HttpContextWithSufficientClaims = GetHttpContextWithSufficientClaims();
        HttpContextWithInsufficientClaims = GetHttpContextWithInsufficientClaims();
    }

	public UserService UserService { get; }

	public Mock<IHttpContextAccessor> MockHttpContextAccessor { get; }
	public Mock<ILogger<UserService>> MockLogger { get; }

    public string Name { get; }
    public User User { get; }
    public HttpContext HttpContextWithSufficientClaims { get; }
    public HttpContext HttpContextWithInsufficientClaims { get; }

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
    private User GetUser()
    {
        return new User()
        {
            Id = 1,
            UserName = Name,
            PasswordHash = Name,
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
