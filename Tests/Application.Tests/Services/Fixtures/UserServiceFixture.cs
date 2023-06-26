using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
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

        var userFaker = new Faker<User>()
            .CustomInstantiator(f => new(
                f.Name.FirstName(),
                f.Name.LastName(),
                f.Internet.Email(),
                f.Internet.UserName(),
                f.Phone.PhoneNumber(),
                null));

        GeneralFaker = new Faker();

        MockHttpContextAccessor = fixture.Freeze<Mock<IHttpContextAccessor>>();
		MockLogger = fixture.Freeze<Mock<ILogger<UserService>>>();

		UserService = new UserService(
			MockHttpContextAccessor.Object,
			MockLogger.Object);

        User = userFaker.Generate();
        HttpContextWithSufficientClaims = GetHttpContextWithSufficientClaims();
        HttpContextWithInsufficientClaims = GetHttpContextWithInsufficientClaims();
    }

    private Faker GeneralFaker { get; }

    public UserService UserService { get; }

	public Mock<IHttpContextAccessor> MockHttpContextAccessor { get; }
	public Mock<ILogger<UserService>> MockLogger { get; }

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

    private IEnumerable<Claim> GetInsufficientClaims() =>
        new List<Claim>()
        {
            new Claim(ClaimTypes.Name, GeneralFaker.Name.FullName()),
            new Claim(ClaimTypes.Email, GeneralFaker.Internet.Email()),
            new Claim(ClaimTypes.Role, GeneralFaker.Name.JobTitle())
        };

    private IEnumerable<Claim> GetSufficientClaims() =>
        new List<Claim>()
        {
            new Claim(ClaimTypes.Name, GeneralFaker.Name.FullName()),
            new Claim(ClaimTypes.Email, GeneralFaker.Internet.Email()),
            new Claim(ClaimTypes.Role, "Admin")
        };

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
