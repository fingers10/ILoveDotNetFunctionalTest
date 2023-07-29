using API.Services;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace FunctionalTest.Fixtures
{
    public class ExternalAPIFixture : IDisposable
    {
        private readonly WireMockServer WireMockServer;

        public ExternalAPIFixture()
        {
            WireMockServer = WireMockServer.Start(50000, true);
        }

        public void Dispose()
        {
            WireMockServer.Stop();
        }

        public void Reset()
        {
            WireMockServer.Reset();
        }

        public void SetupGetWeather(ExternalWeatherForecast responseBodyResource, int statusCode = 200)
        {
            var request = Request.Create()
                .WithPath("/v1/forecast")
                .UsingGet();

            WireMockServer.Given(request)
                .RespondWith(
                    Response.Create()
                    .WithStatusCode(statusCode)
                    .WithHeader("content-type", "application/json")
                    .WithBodyAsJson(responseBodyResource!)
                );
        }
    }
}

