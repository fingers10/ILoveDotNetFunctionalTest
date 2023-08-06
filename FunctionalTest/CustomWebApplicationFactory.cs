using API.DbContexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace FunctionalTest;
public class CustomWebApplicationFactory
{
    private static WebApplicationFactory<Program> Application(
        IConfiguration? configuration = null,
        Action<IServiceCollection>? testServices = null)
        => new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test");

                var projectDir = Directory.GetCurrentDirectory();

                builder.ConfigureAppConfiguration((context, conf) =>
                {
                    conf.AddJsonFile(Path.Combine(projectDir, "appsettings.Test.json"));
                });

                if (configuration is not null)
                {
                    builder.UseConfiguration(configuration);
                }

                builder.ConfigureTestServices(async services =>
                {
                    services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

                    services.AddScoped(_ => AuthClaimsProvider.WithGuestClaims());

                    if (testServices is not null)
                    {
                        testServices(services);
                    }

                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(WeatherForecastDbContext));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
                    var connection = new SqliteConnection(connectionStringBuilder.ToString());

                    var dbContextOptions = new DbContextOptionsBuilder<WeatherForecastDbContext>()
                                            .UseSqlite(connection)
                                            .Options;

                    services.AddScoped(options => new WeatherForecastDbContext(dbContextOptions));

                    await connection.CloseAsync();

                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<WeatherForecastDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory>>();

                    try
                    {
                        await db.Database.OpenConnectionAsync();
                        await db.Database.EnsureCreatedAsync();
                        DatabaseHelper.ResetDbForTests(db);
                        DatabaseHelper.InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"An error occurred seeding the database with test data. Error: {ex.Message}");
                        throw;
                    }
                });
            });

    protected static async Task RunTest(
        Func<HttpClient, Task> test,
        IConfiguration? configuration = null,
        Action<IServiceCollection>? services = null)
    {
        var client = Application(configuration, services)
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        await test(client);
    }
}
