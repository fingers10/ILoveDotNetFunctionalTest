using API.DbContexts;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController() : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("fromdatabase", Name = "GetWeatherForecastFromDatabase")]
    public IEnumerable<WeatherForecast> GetFromDatabase([FromServices] WeatherForecastDbContext dbContext)
    {
        return [.. dbContext.WeatherForecasts];
    }

    [HttpGet("fromapi", Name = "GetWeatherForecastFromAPI")]
    public async Task<WeatherForecast> GetFromAPI([FromServices] IExternalAPIService apiService)
    {
        return await apiService.GetWeatherForecast();
    }
}
