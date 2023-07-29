using API;
using API.DbContexts;

namespace FunctionalTest;

public static class DatabaseHelper
{
    public static void InitializeDbForTests(WeatherForecastDbContext db)
    {
        db.WeatherForecasts.Add(
            new WeatherForecast
            {
                Id = 1,
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                TemperatureC = 1,
                Summary = "Freezing from test"
            });
        db.SaveChanges();
    }

    public static void ResetDbForTests(WeatherForecastDbContext db)
    {
        db.WeatherForecasts.RemoveRange(db.WeatherForecasts);
        db.SaveChanges();
    }
}
