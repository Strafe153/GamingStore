using Application.Tests.Services.Fixtures;
using Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Services;

public class UserServiceTests : IClassFixture<UserServiceFixture>
{
    private readonly UserServiceFixture _fixture;

	public UserServiceTests(UserServiceFixture fixture)
	{
		_fixture = fixture;
	}

	[Fact]
	public void VerifyUserAccessRights_Should_ReturnVoid_WhenClaimsAreSufficient()
	{
        // Arrange
        _fixture.MockHttpContextAccessor
            .Setup(a => a.HttpContext)
            .Returns(_fixture.HttpContextWithSufficientClaims);

        // Act
        var result = () => _fixture.UserService.VerifyUserAccessRights(_fixture.User);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void VerifyUserAccessRights_Should_ThrowNotEnoughRightsException_WhenClaimsAreInsufficient()
    {
        // Arrange
        _fixture.MockHttpContextAccessor
            .Setup(a => a.HttpContext)
            .Returns(_fixture.HttpContextWithInsufficientClaims);

        // Act
        var result = () => _fixture.UserService.VerifyUserAccessRights(_fixture.User);

        // Assert
        result.Should().Throw<NotEnoughRightsException>();
    }
}
