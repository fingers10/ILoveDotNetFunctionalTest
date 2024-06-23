using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit.Abstractions;

namespace FunctionalTest;

[ExcludeFromCodeCoverage]
public static class HttpClientExtensions
{
    private static readonly JsonSerializerOptions _jsonPrintOptions =
        new() { WriteIndented = true };

    private static readonly JsonSerializerOptions _jsonDeserializeOptions =
        new(JsonSerializerDefaults.Web) { Converters = { new JsonStringEnumConverter() } };

    public static async Task<T> GetJsonResultAsync<T>(this HttpClient client, string url,
        HttpStatusCode expectedStatus, ITestOutputHelper output)
    {
        var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        return await DeserializeAndCheckResponse<T>(response, expectedStatus, output);
    }

    private static async Task<T> DeserializeAndCheckResponse<T>(HttpResponseMessage response,
        HttpStatusCode expectedStatus, ITestOutputHelper output)
    {
        using var streamContent = await response.Content.ReadAsStreamAsync();
        try
        {
            var result = await JsonSerializer.DeserializeAsync<T>(streamContent, _jsonDeserializeOptions);
            response.StatusCode.Should().Be(expectedStatus);
            result.Should().NotBeNull();
            return result!;
        }
        catch (Exception)
        {
            var stringContent = await response.Content.ReadAsStringAsync();
            WriteOutput(stringContent, output);
            throw;
        }
    }

    private static void WriteOutput(string stringContent, ITestOutputHelper output)
    {
        string? outputText;
        try
        {
            var jsonContent = JsonDocument.Parse(stringContent);
            outputText = JsonSerializer.Serialize(jsonContent, _jsonPrintOptions);
        }
        catch
        {
            outputText = stringContent;
        }
        output.WriteLine(outputText);
    }
}