using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Authentication;
using AutoFixture;
using DocumentsStore.Api.DTOs.Groups;
using DocumentsStore.Api.DTOs.Users;
using DocumentsStore.Domain;

namespace DocumentsStore.IntegrationTests.Helpers;

public static class HttpClientExtensions
{
    public static async Task Authenticate(this HttpClient httpClient, int userId = 1)
    {
        var response = await httpClient.PostAsJsonAsync("/api/auth/login", new
        {
            Id = userId
        });
        
        var token = await response.Content.ReadAsStringAsync();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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

    public static async Task DeleteUser(this HttpClient httpClient, int userId)
    {
        var response = await httpClient.DeleteAsync($"/api/users/{userId}");
        response.EnsureSuccessStatusCode();
    }
    
    public static async Task<GroupDto> CreateGroup(this HttpClient httpClient)
    {
        var fixture = new Fixture();
        var payload = fixture.Create<GroupDto>();
        
        var response = await httpClient.PostAsJsonAsync("/api/groups", payload);
        
        response.EnsureSuccessStatusCode();
        var group = await response.Content.ReadFromJsonAsync<GroupDto>(JsonSerializer.Default);
        Assert.NotNull(group);

        return group;
    }

    public static async Task DeleteGroup(this HttpClient httpClient, int groupId)
    {
        var response = await httpClient.DeleteAsync($"/api/groups/{groupId}");
        response.EnsureSuccessStatusCode();
    }
}