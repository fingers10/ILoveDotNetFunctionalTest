using API.Services;

namespace FunctionalTest;
internal class FakeExternalAPIService : IExternalAPIService
{
	public string GetResult()
	{
		return "Hello from fake external API";
	}
}
