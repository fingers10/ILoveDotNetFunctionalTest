using API.DbContexts;
using WireMock.Server;

namespace FunctionalTest;

public abstract class BaseTest(CustomWebApiFactory factory)
{
    protected CustomWebApiFactory Factory => factory;
    protected HttpClient Client => factory.CreateClient();
    protected WeatherForecastDbContext Database => factory.SharedFixture.DbContext;
    protected WireMockServer WeatherService => factory.SharedFixture.WeatherService;
}