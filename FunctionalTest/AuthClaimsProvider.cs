using System.Security.Claims;

namespace FunctionalTest;
public class AuthClaimsProvider
{
    public IList<Claim> Claims { get; }

    public AuthClaimsProvider(IList<Claim> claims)
    {
        Claims = claims;
    }

    public AuthClaimsProvider()
    {
        Claims = [];
    }

    public static AuthClaimsProvider WithGuestClaims()
    {
        var provider = new AuthClaimsProvider();
        provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
        provider.Claims.Add(new Claim(ClaimTypes.Name, "Guest User"));
        provider.Claims.Add(new Claim(ClaimTypes.Role, "Guest"));

        return provider;
    }

    public static AuthClaimsProvider WithAdminClaims()
    {
        var provider = new AuthClaimsProvider();
        provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.Empty.ToString()));
        provider.Claims.Add(new Claim(ClaimTypes.Name, "Admin User"));
        provider.Claims.Add(new Claim(ClaimTypes.Role, "Admin"));

        return provider;
    }
}
