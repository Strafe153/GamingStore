using IdentityModel;
using IdentityServer4.Models;

namespace IdentityServer;

public static class Configuration
{
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResource(
                name: "profile",
                userClaims: new [] { "name" },
                displayName: "User's profile data")
        };
    }

    public static IEnumerable<ApiResource> GetApiResources()
    {
        return new List<ApiResource>()
        {
            new ApiResource("gamingStoreApi", "Gaming Store")
            {
                Scopes = { "apiAccess" }
            }
        };
    }

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new[]
        {
            new ApiScope("apiAccess", "Gaming Store Access")
        };
    }

    public static IEnumerable<Client> GetClients()
    {
        return new[]
        {
            new Client()
            {
                RequireConsent = false,
                ClientId = "postman_client",
                ClientName = "Postman Client",
                AllowedScopes = { "apiAccess", "openid" },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                ClientSecrets = { new Secret("test_client_secret".ToSha256()) },
                AccessTokenLifetime = 600
            }
        };
    }
}
