using System.Text.Json;
using System.Text.Json.Serialization;

namespace DocumentsStore.IntegrationTests.Helpers;

public static class JsonSerializer
{
    public static JsonSerializerOptions Default = new JsonSerializerOptions() { Converters = { new JsonStringEnumConverter() }, PropertyNameCaseInsensitive = true };
}