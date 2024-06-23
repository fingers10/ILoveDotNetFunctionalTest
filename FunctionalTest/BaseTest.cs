using API.DbContexts;

namespace FunctionalTest;

public abstract class BaseTest(CustomWebApiFactory factory)
{
    protected CustomWebApiFactory Factory => factory;
    protected HttpClient Client => factory.CreateClient();
    protected WeatherForecastDbContext Database => factory.SharedFixture.DbContext;
}