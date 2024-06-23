using API.DbContexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FunctionalTest;
public class SharedFixture : IAsyncLifetime
{
    public const string DatabaseName = "InMemTestDb";
    public WeatherForecastDbContext DbContext = default!;

    public async Task InitializeAsync()
    {
        // Sqlite
        var connectionStringBuilder = new SqliteConnectionStringBuilder
        {
            DataSource = DatabaseName,
            Mode = SqliteOpenMode.Memory,
            Cache = SqliteCacheMode.Shared
        };
        var connection = new SqliteConnection(connectionStringBuilder.ToString());

        var dbContextOptions = new DbContextOptionsBuilder<WeatherForecastDbContext>()
                                .UseSqlite(connection)
                                .Options;

        DbContext = new WeatherForecastDbContext(dbContextOptions);

        try
        {
            await DbContext.Database.EnsureDeletedAsync();
            await DbContext.Database.OpenConnectionAsync();
            await DbContext.Database.EnsureCreatedAsync();
            await DatabaseHelper.ResetDbForTestsAsync(DbContext);
            await DatabaseHelper.InitializeDbForTestsAsync(DbContext);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DisposeAsync()
    {
        if (DbContext is not null)
        {
            await DbContext.DisposeAsync();
        }
    }
}