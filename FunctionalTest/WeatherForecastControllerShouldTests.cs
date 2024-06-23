using API;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Net;
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
}
