using System.Net.Http.Json;
using AutoFixture;
using DocumentsStore.Api.DTOs.Groups;
using DocumentsStore.Api.DTOs.Users;
using DocumentsStore.Domain;
using DocumentsStore.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DocumentsStore.IntegrationTests;

public class GroupsTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _testFixtureBase;
    private readonly HttpClient _client;
    private readonly Fixture _fixture;
    private UserDto _adminGroup;
    private GroupDto _group;

    public GroupsTests(WebApplicationFactory<Program> testFixtureBase)
    {
        _testFixtureBase = testFixtureBase;
        _fixture = new Fixture();
        _client = _testFixtureBase.CreateDefaultClient();
    }
    
    public async Task InitializeAsync()
    {
        _adminGroup = await _client.CreateAndLoginUser(Role.Admin);
        _group = await _client.CreateGroup();
    }

    public async Task DisposeAsync()
    {
        // await _client.DeleteGroup(_user.Id);
        await _client.DeleteUser(_adminGroup.Id);
    }

    [Fact]
    public async Task CreateGroup_Success()
    {
        // Arrange
        var payload = _fixture.Create<GroupDto>();
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/groups", payload);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var user = await response.Content.ReadFromJsonAsync<GroupDto>(JsonSerializer.Default);
        Assert.NotNull(user);
        Assert.Equal(payload.Name, user.Name);
    }
    
    [Fact]
    public async Task GetGroupById_Success()
    {
        // Arrange
        
        // Act
        var response = await _client.GetAsync($"/api/groups/{_group.Id}");

        // Assert
        var group = await response.Content.ReadFromJsonAsync<GroupDto>(JsonSerializer.Default);
        Assert.NotNull(group);
    }
    
    [Fact]
    public async Task GetAllGroups_Success()
    {
        // Arrange
        
        // Act
        var response = await _client.GetAsync($"/api/groups");

        // Assert
        var users = await response.Content.ReadFromJsonAsync<IEnumerable<GroupDto>>(JsonSerializer.Default);
        Assert.NotNull(users);
        Assert.NotEmpty(users);
    }

    [Fact]
    public async Task UpdateGroup_Success()
    {
        // Arrange
        var payload = _fixture.Create<GroupDto>();
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/groups/{_group.Id}", payload);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var group = await response.Content.ReadFromJsonAsync<GroupDto>(JsonSerializer.Default);
        Assert.NotNull(group);
        Assert.Equal(payload.Name, group.Name);
    }
    
    [Fact]
    public async Task DeleteGroup_Success()
    {
        // Arrange
        var group = await _client.CreateGroup();
        
        // Act
        var response = await _client.DeleteAsync($"/api/groups/{group.Id}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var deletedGroup = await response.Content.ReadFromJsonAsync<GroupDto>(JsonSerializer.Default);
        Assert.NotNull(deletedGroup);
        Assert.Equal(group.Id, deletedGroup.Id);
    }
}