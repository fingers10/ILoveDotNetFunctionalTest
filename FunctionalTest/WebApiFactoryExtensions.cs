using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace FunctionalTest;

public static class WebApiFactoryExtensions
{
    public static HttpClient CreateClientWithClaim(this WebApplicationFactory<Program> factory, AuthClaimsProvider claims)
    {
        var client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(AuthClaimsProvider));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                    services.AddScoped(_ => claims);
                }
            });
        })
        .CreateClient();

        return client;
    }
}