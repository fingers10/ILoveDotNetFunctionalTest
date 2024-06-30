using API;
using API.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit.Abstractions;

namespace FunctionalTest;
[ExcludeFromCodeCoverage]
[Collection(nameof(FunctionalTestCollection))]
public class WeatherForecastControllerShouldTests(
    CustomWebApiFactory factory,
    ITestOutputHelper outputHelper) : BaseTest(factory), IClassFixture<CustomWebApiFactory>
{
    [Fact]
    public async Task ReturnExpectedResponse()
    {
        var result = await Client.GetJsonResultAsync<List<WeatherForecast>>("/weatherforecast", HttpStatusCode.OK, outputHelper);
        result.Count.Should().Be(5);
    }

    [Fact]
    public async Task ReturnExpectedResponse_From_Database()
    {
        var expectedWeatherForecast = await Database.WeatherForecasts.FirstAsync();

        var result = await Client.GetJsonResultAsync<List<WeatherForecast>>("/weatherforecast/fromdatabase", HttpStatusCode.OK, outputHelper);
        result.Count.Should().Be(1);
        result[0].Should().BeEquivalentTo(expectedWeatherForecast);
    }

    [Fact]
    public async Task ReturnExpectedResponse_From_API()
    {
        var expectedWeather = new ExternalWeatherForecast { Current_Weather = new CurrentWeather { Temperature = 18 } };
        WeatherService
            .Given(Request.Create().WithPath("/v1/forecast").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK).WithBodyAsJson(expectedWeather));

        var result = await Client.GetJsonResultAsync<WeatherForecast>("/weatherforecast/fromapi", HttpStatusCode.OK, outputHelper);
        
        result.Should().NotBeNull();
        result.TemperatureC.Should().Be((int)expectedWeather.Current_Weather.Temperature);
        result.Date.Should().Be(DateOnly.FromDateTime(DateTime.Now));
    }
}
