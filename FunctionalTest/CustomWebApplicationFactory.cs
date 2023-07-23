using API.DbContexts;
using API.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FunctionalTest;
public class CustomWebApplicationFactory
{
	private static WebApplicationFactory<Program> Application
		=> new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder =>
			{
				builder.UseEnvironment("Test");

				builder.ConfigureTestServices(async services =>
				{
					var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IExternalAPIService));

					if (descriptor != null)
					{
						services.Remove(descriptor);
					}

					services.AddTransient<IExternalAPIService, FakeExternalAPIService>();

					descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(WeatherForecastDbContext));

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

	protected static async Task RunTest(Func<HttpClient, Task> test)
	{
		var client = Application.CreateClient(new WebApplicationFactoryClientOptions
		{
			AllowAutoRedirect = false
		});
		await test(client);
	}
}
