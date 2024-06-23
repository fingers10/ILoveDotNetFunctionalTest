using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace FunctionalTest;
public class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    AuthClaimsProvider claimsProvider) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "Test";
    private readonly IList<Claim> _claims = claimsProvider.Claims;

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var identity = new ClaimsIdentity(_claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        // If needed to add anything in HttpContext to be available inside app, then we can do it below.
        // the below example uses claims to set value. But it can be anything. 
        // Just Inject _httpContextAccessor in this class and use.
        // _httpContextAccessor.HttpContext.Items[key] = principal.Claims.First(claim => claim.Type == key)?.Value;

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}
