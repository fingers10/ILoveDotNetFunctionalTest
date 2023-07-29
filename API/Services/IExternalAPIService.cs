namespace API.Services;

public interface IExternalAPIService
{
    string GetResult();

    Task<WeatherForecast> GetWeatherForecast();
}
