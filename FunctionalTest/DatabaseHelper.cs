using API;
using API.DbContexts;

namespace FunctionalTest;

public static class DatabaseHelper
{
    public static async Task InitializeDbForTestsAsync(WeatherForecastDbContext db)
    {
        await db.WeatherForecasts.AddAsync(
            new WeatherForecast
            {
                Id = 1,
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                TemperatureC = 1,
                Summary = "Freezing from database"
            });
        await db.SaveChangesAsync();
    }

    public static async Task ResetDbForTestsAsync(WeatherForecastDbContext db)
    {
        db.WeatherForecasts.RemoveRange(db.WeatherForecasts);
        await db.SaveChangesAsync();
    }
}
