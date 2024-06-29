using API.DbContexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace FunctionalTest;
public class SharedFixture : IAsyncLifetime
{
    //public const string DatabaseName = "InMemoryTestDb";
    public WeatherForecastDbContext DbContext = default!;
    public string PostgresConnectionString => _dbContainer.GetConnectionString();
    private readonly PostgreSqlContainer _dbContainer =
        new PostgreSqlBuilder()
        .WithDatabase("ILoveDotNet")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    public async Task InitializeAsync()
    {
        // PostgreSQL
        await _dbContainer.StartAsync();

        var dbContextOptions = new DbContextOptionsBuilder<WeatherForecastDbContext>()
                                .UseNpgsql(PostgresConnectionString)
                                .Options;

        DbContext = new WeatherForecastDbContext(dbContextOptions);

        await DbContext.Database.MigrateAsync();

        // Sqlite
        //var connectionStringBuilder = new SqliteConnectionStringBuilder
        //{
        //    DataSource = DatabaseName,
        //    Mode = SqliteOpenMode.Memory,
        //    Cache = SqliteCacheMode.Shared
        //};
        //var connection = new SqliteConnection(connectionStringBuilder.ToString());

        //var dbContextOptions = new DbContextOptionsBuilder<WeatherForecastDbContext>()
        //                        .UseSqlite(connection)
        //                        .Options;

        //DbContext = new WeatherForecastDbContext(dbContextOptions);

        //try
        //{
            //await DbContext.Database.EnsureDeletedAsync();
            //await DbContext.Database.OpenConnectionAsync();
            //await DbContext.Database.EnsureCreatedAsync();
            //await DatabaseHelper.ResetDbForTestsAsync(DbContext);
            //await DatabaseHelper.InitializeDbForTestsAsync(DbContext);
        //}
        //catch (Exception)
        //{
        //    throw;
        //}
    }

    public async Task DisposeAsync()
    {
        if (DbContext is not null)
        {
            await DbContext.DisposeAsync();
        }

        if (_dbContainer is not null)
        {
            await _dbContainer.StopAsync();
            await _dbContainer.DisposeAsync();
        }

    }
}