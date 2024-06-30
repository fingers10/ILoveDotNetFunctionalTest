namespace API.Services;

public interface IExternalAPIService
{
    Task<WeatherForecast> GetWeatherForecast();
}