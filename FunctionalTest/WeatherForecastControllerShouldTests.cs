using API;
using FluentAssertions;
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

    //[Fact]
    //public Task ReturnExpectedResponseFromDatabase()
    //{
    //    return RunTest(async (client) =>
    //    {
    //        using var request = new HttpRequestMessage(HttpMethod.Get, $"/weatherforecast/fromdatabase");
    //        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

    //        var responseStream = await response.Content.ReadAsStreamAsync();
    //        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    //        var result = await JsonSerializer.DeserializeAsync<List<WeatherForecast>>(responseStream, options);

    //        response.StatusCode.Should().Be(HttpStatusCode.OK);
    //        result!.Count.Should().Be(1);
    //        result[0].Summary.Should().Be("Freezing from test");
    //    },
    //    null,
    //    (services) =>
    //    {
    //        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IExternalAPIService));

    //        if (descriptor != null)
    //        {
    //            services.Remove(descriptor);
    //        }

    //        services.AddTransient<IExternalAPIService, FakeExternalAPIService>();
    //    });
    //}

    //[Fact]
    //public Task ReturnExpectedResponseFromAPI()
    //{
    //    var expectedWeather = new ExternalWeatherForecast { Current_Weather = new CurrentWeather { Temperature = 18 } };

    //    _weatherFixture.SetupGetWeather(expectedWeather);

    //    return RunTest(async (client) =>
    //    {
    //        using var request = new HttpRequestMessage(HttpMethod.Get, $"/weatherforecast/fromapi");
    //        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

    //        var responseStream = await response.Content.ReadAsStreamAsync();
    //        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    //        var result = await JsonSerializer.DeserializeAsync<WeatherForecast>(responseStream, options);

    //        response.StatusCode.Should().Be(HttpStatusCode.OK);
    //        result!.TemperatureC.Should().Be((int)expectedWeather.Current_Weather.Temperature);
    //        result.Date.Should().Be(DateOnly.FromDateTime(DateTime.Now));
    //    },
    //    null,
    //    (services) =>
    //    {
    //        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(AuthClaimsProvider));

    //        if (descriptor != null)
    //        {
    //            services.Remove(descriptor);
    //            services.AddScoped(_ => AuthClaimsProvider.WithAdminClaims());
    //        }
    //    });
    //}
}