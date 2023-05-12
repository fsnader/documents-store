using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DocumentsStore.IntegrationTests.Authentication;

public static class HttpClientExtensions
{
    public static async Task<HttpClient> Authenticate(this HttpClient httpClient, int userId = 1)
    {
        var response = await httpClient.PostAsJsonAsync("/api/auth/login", new
        {
            Id = userId
        });
        
        var token = await response.Content.ReadAsStringAsync();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return httpClient;
    }
}