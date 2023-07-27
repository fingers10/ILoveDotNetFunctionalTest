using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Services;

public class ExternalAPIService : IExternalAPIService
{
    private readonly IHttpClientFactory httpClientFactory;

    public ExternalAPIService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public string GetResult()
    {
        return "Hello from external API";
    }

    public async Task<WeatherForecast> GetWeatherForevast()
    {
        var client = httpClientFactory.CreateClient("WeatherClient");

        var response = await client.GetFromJsonAsync<ExternalWeatherForecast>("v1/forecast?latitude=52.52&longitude=13.41&current_weather=true");

        var weatherForecast = new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = (int)response!.Current_Weather.Temperature
        };

        return weatherForecast;
    }
}

public class ExternalWeatherForecast
{
    [JsonPropertyName("current_weather")]
    public CurrentWeather Current_Weather { get; set; }
}

public class CurrentWeather
{
    [JsonPropertyName("temperature")]
    public decimal Temperature { get; set; }
}
