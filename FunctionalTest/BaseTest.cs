using API.DbContexts;

namespace FunctionalTest;

public abstract class BaseTest(CustomWebApiFactory factory)
{
    protected CustomWebApiFactory Factory => factory;
    protected HttpClient Client => factory.CreateClient();
    protected SharedFixture SharedContext => factory.SharedFixture;
    protected WeatherForecastDbContext Database => factory.SharedFixture.DbContext;
}
