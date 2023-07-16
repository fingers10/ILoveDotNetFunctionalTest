using Microsoft.AspNetCore.Mvc.Testing;

namespace FunctionalTest;
public class CustomWebApplicationFactory
{
    private static WebApplicationFactory<Program> Application => new WebApplicationFactory<Program>();

    protected static async Task RunTest(Func<HttpClient, Task> test)
    {
        var client = Application.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        await test(client);
    }
}
