using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoFixture;
using DocumentsStore.Api.DTOs.Users;
using DocumentsStore.Domain;

namespace DocumentsStore.IntegrationTests;

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

    public static async Task<UserDto> CreateUser(this HttpClient httpClient, Role role)
    {
        var fixture = new Fixture();
        var payload = new CreateUserDto
        {
            Name = fixture.Create<string>(),
            Email = fixture.Create<string>(),
            Role = role,
        };
        
            
        var response = await httpClient.PostAsJsonAsync("/api/auth/signup", payload);
        
        var user = await response.Content.ReadFromJsonAsync<UserDto>(JsonSerializer.Default);
        
        if (user is null)
        {
            throw new AuthenticationException();
        }

        return user;
    }

    public static async Task<UserDto> CreateAndLoginUser(this HttpClient httpClient, Role role)
    {
        var user = await httpClient.CreateUser(role);
        await httpClient.Authenticate(user.Id);
        return user;
    }
}