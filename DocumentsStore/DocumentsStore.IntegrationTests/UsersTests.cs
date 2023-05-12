using System.Net.Http.Json;
using System.Security.Authentication;
using AutoFixture;
using DocumentsStore.Api.DTOs.Groups;
using DocumentsStore.Api.DTOs.Users;
using DocumentsStore.Domain;
using DocumentsStore.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DocumentsStore.IntegrationTests;

public class UsersTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _testFixtureBase;
    private readonly HttpClient _client;
    private readonly Fixture _fixture;
    private UserDto _adminUser;
    private UserDto _user;

    public UsersTests(WebApplicationFactory<Program> testFixtureBase)
    {
        _testFixtureBase = testFixtureBase;
        _fixture = new Fixture();
        _client = _testFixtureBase.CreateDefaultClient();
    }
    
    public async Task InitializeAsync()
    {
        _adminUser = await _client.CreateAndLoginUser(Role.Admin);
        _user = await _client.CreateUser(Role.Regular);
    }

    public async Task DisposeAsync()
    {
        await _client.DeleteUser(_user.Id);
        await _client.DeleteUser(_adminUser.Id);
    }

    [Fact]
    public async Task CreateUser_Success()
    {
        // Arrange
        var payload = _fixture.Create<CreateUserDto>();
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/users", payload);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var user = await response.Content.ReadFromJsonAsync<UserDto>(JsonSerializer.Default);
        Assert.NotNull(user);
        Assert.Equal(payload.Name, user.Name);
    }
    
    [Fact]
    public async Task GetUserById_Success()
    {
        // Arrange
        
        // Act
        var response = await _client.GetAsync($"/api/users/{_user.Id}");

        // Assert
        var user = await response.Content.ReadFromJsonAsync<UserWithGroupsDto>(JsonSerializer.Default);
        Assert.NotNull(user);
    }
    
    [Fact]
    public async Task GetAllUsers_Success()
    {
        // Arrange
        
        // Act
        var response = await _client.GetAsync($"/api/users");

        // Assert
        var users = await response.Content.ReadFromJsonAsync<IEnumerable<UserWithGroupsDto>>(JsonSerializer.Default);
        Assert.NotNull(users);
        Assert.NotEmpty(users);
    }

    [Fact]
    public async Task UpdateUser_Success()
    {
        // Arrange
        var payload = _fixture.Create<CreateUserDto>();
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/users/{_user.Id}", payload);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var user = await response.Content.ReadFromJsonAsync<UserDto>(JsonSerializer.Default);
        Assert.NotNull(user);
        Assert.Equal(payload.Name, user.Name);
    }
    
    [Fact]
    public async Task DeleteUser_Success()
    {
        // Arrange
        var user = await _client.CreateUser(Role.Regular);
        
        // Act
        var response = await _client.DeleteAsync($"/api/users/{user.Id}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var deletedUser = await response.Content.ReadFromJsonAsync<UserDto>(JsonSerializer.Default);
        Assert.NotNull(deletedUser);
        Assert.Equal(user.Id, deletedUser.Id);
    }

    [Fact]
    public async Task AddUserToGroup_Success()
    {
        // Arrange
        var group = await _client.CreateGroup();
        
        // Act
        var response = await _client.PostAsJsonAsync($"/api/users/{_user.Id}/groups/{group.Id}", new {});

        // Assert
        response.EnsureSuccessStatusCode();
        var userWithGroups = await response.Content.ReadFromJsonAsync<UserWithGroupsDto>(JsonSerializer.Default);
        Assert.NotNull(userWithGroups);
        Assert.NotNull(userWithGroups.Groups);
        Assert.Contains(group.Id, userWithGroups.Groups.Select(g => g.Id));

        await _client.DeleteGroup(group.Id);
    }

    [Fact]
    public async Task RemoveUserFromGroup_Success()
    {
        var group = await AddUserToNewGroup(_user.Id);
        
        // Act
        var response = await _client.DeleteAsync($"/api/users/{_user.Id}/groups/{group.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var userWithGroups = await response.Content.ReadFromJsonAsync<UserWithGroupsDto>(JsonSerializer.Default);
        Assert.NotNull(userWithGroups);
        Assert.NotNull(userWithGroups.Groups);
        Assert.DoesNotContain(group.Id, userWithGroups.Groups.Select(g => g.Id));
        
        await _client.DeleteGroup(group.Id);
    }

    [Fact]
    public async Task GetUsersGroups_Success()
    {
        // Arrange
        var group = await AddUserToNewGroup(_user.Id);
        
        // Act
        var response = await _client.GetAsync($"/api/users/{_user.Id}/groups");

        // Assert
        response.EnsureSuccessStatusCode();
        var groups = await response.Content.ReadFromJsonAsync<IEnumerable<GroupDto>>(JsonSerializer.Default);
        Assert.NotNull(groups);
        Assert.NotEmpty(groups);
        
        await _client.DeleteGroup(group.Id);
    }
    
    private async Task<GroupDto> AddUserToNewGroup(int userId)
    {
        var group = await _client.CreateGroup();
        
        var response = await _client.PostAsJsonAsync($"/api/users/{userId}/groups/{group.Id}", new {});

        response.EnsureSuccessStatusCode();
        return group;
    }
}