using API;
using API.Services;

namespace FunctionalTest;
internal class FakeExternalAPIService : IExternalAPIService
{
    public string GetResult()
    {
        return "Hello from fake external API";
    }

    public Task<WeatherForecast> GetWeatherForecast()
    {
        throw new NotImplementedException();
    }
}
