using API;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace FunctionalTest;
public class WeatherForecastControllerShouldTests : CustomWebApplicationFactory
{
    [Fact]
    public Task ReturnExpectedResponse()
    {
        return RunTest(async (client) =>
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"/weatherforecast");
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            var responseStream = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var result = await JsonSerializer.DeserializeAsync<List<WeatherForecast>>(responseStream, options);
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result!.Count.Should().Be(5);
        });
    }
}