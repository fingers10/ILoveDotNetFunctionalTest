using API;
using API.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
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

	[Fact]
	public Task ReturnExpectedResponseFromDatabase()
	{
		return RunTest(async (client) =>
		{
			using var request = new HttpRequestMessage(HttpMethod.Get, $"/weatherforecast/fromdatabase");
			var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

			var responseStream = await response.Content.ReadAsStreamAsync();
			var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
			var result = await JsonSerializer.DeserializeAsync<List<WeatherForecast>>(responseStream, options);

			response.StatusCode.Should().Be(HttpStatusCode.OK);
			result!.Count.Should().Be(1);
			result[0].Summary.Should().Be("Freezing from test");
		},
		null,
		(services) =>
		{
			var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IExternalAPIService));

			if (descriptor != null)
			{
				services.Remove(descriptor);
			}

			services.AddTransient<IExternalAPIService, FakeExternalAPIService>();
		});
	}
}