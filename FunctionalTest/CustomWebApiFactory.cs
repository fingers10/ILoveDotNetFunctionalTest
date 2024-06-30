using API.DbContexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FunctionalTest;
public class CustomWebApiFactory(SharedFixture fixture) : WebApplicationFactory<Program>
{
    public SharedFixture SharedFixture => fixture;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureAppConfiguration((context, conf) =>
        {
            conf.AddInMemoryCollection(new Dictionary<string, string>
            {
                ["WeatherAPIUrl"] = $"{SharedFixture.WeatherServiceUrl}/"
            }!);
        });

        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(TestAuthHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });

            services.AddScoped(_ => new AuthClaimsProvider());
        });

        builder.ConfigureServices(services =>
        {
            var ctx = services.SingleOrDefault(d => d.ServiceType == typeof(WeatherForecastDbContext));
            services.Remove(ctx!);

            //var connectionStringBuilder = new SqliteConnectionStringBuilder
            //{
            //    DataSource = SharedFixture.DatabaseName,
            //    Mode = SqliteOpenMode.Memory,
            //    Cache = SqliteCacheMode.Shared
            //};
            //var connection = new SqliteConnection(connectionStringBuilder.ToString());

            //// SQLite
            //services.AddDbContext<WeatherForecastDbContext>(opts =>
            //    opts.UseSqlite(connectionStringBuilder.ToString()));

            // PostgreSQL
            services.AddDbContext<WeatherForecastDbContext>(opts =>
                opts.UseNpgsql(SharedFixture.PostgresConnectionString));
        });
    }
}
