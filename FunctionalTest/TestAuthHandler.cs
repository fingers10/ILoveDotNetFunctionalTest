using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace FunctionalTest;
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IList<Claim> _claims;

    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, AuthClaimsProvider claimsProvider)
        : base(options, logger, encoder, clock)
    {
        _claims = claimsProvider.Claims;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var identity = new ClaimsIdentity(_claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        // If needed to add anything in HttpContext to be available inside app, then we can do it below.
        // the below example uses claims to set value. But it can be anything. 
        // Just Inject _httpContextAccessor in this class and use.
        // _httpContextAccessor.HttpContext.Items[key] = principal.Claims.First(claim => claim.Type == key)?.Value;

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}
