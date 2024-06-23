using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FunctionalTest;
public class CustomWebApiFactory(SharedFixture fixture) : WebApplicationFactory<Program>
{
    public SharedFixture SharedFixture => fixture;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var projectDir = Directory.GetCurrentDirectory();

        builder.ConfigureAppConfiguration((context, conf) =>
        {
            conf.AddJsonFile(Path.Combine(projectDir, "appsettings.Test.json"));
        });

        builder.UseEnvironment("Test");

        // Added this to avoid actual logging, which was causing cleanup failure as it was trying to make connection with cloudwatch logs somehow
        builder.ConfigureLogging((_, loggingBuilder) => loggingBuilder.ClearProviders());

        builder.ConfigureTestServices(services =>
        {
            //services.AddAuthentication(TestAuthHandler.SchemeName)
            //    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });

            //services.AddScoped<AuthClaimsProvider>();
        });
    }
}
