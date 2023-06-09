using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using DocumentsStore.Api.DTOs.Documents;
using DocumentsStore.Api.DTOs.Users;
using DocumentsStore.Domain;
using DocumentsStore.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DocumentsStore.IntegrationTests
{
    public class CreateDocumentTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly WebApplicationFactory<Program> _testFixtureBase;
        private readonly HttpClient _client;
        private readonly Fixture _fixture;
        private UserDto _user;

        public CreateDocumentTests(WebApplicationFactory<Program> testFixtureBase)
        {
            _testFixtureBase = testFixtureBase;
            _fixture = new Fixture();
            _client = _testFixtureBase.CreateDefaultClient();
        }
        
        public async Task InitializeAsync() => _user = await _client.CreateAndLoginUser(Role.Admin);
        public async Task DisposeAsync() => await _client.DeleteUser(_user.Id);

        [Fact]
        public async Task CreateDocument_Should_Succeed()
        {
            // Arrange   
            var document = _fixture.Create<CreateDocumentDto>();
            document.AuthorizedGroups = Array.Empty<int>();
            document.AuthorizedUsers = new []{ _user.Id };
            
            // Act
            var response = await _client.PostAsJsonAsync("/api/documents", document);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync<DocumentDto>(JsonSerializer.Default);
            Assert.Equal(document.Name, content.Name);
        }
        
        [Fact]
        public async Task CreateDocument_WithInvalid_Payload_Should_Fail()
        {
            // Arrange 
            var document = _fixture.Create<CreateDocumentDto>();
            document.AuthorizedGroups = Array.Empty<int>();
            document.AuthorizedUsers = Array.Empty<int>();
            
            // Act
            var response = await _client.PostAsJsonAsync("/api/documents", document);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ListDocuments_Should_Succeed()
        {
            // Act
            var response = await _client.GetAsync("/api/documents");

            var content = await response.Content.ReadFromJsonAsync<IEnumerable<DocumentDto>>(JsonSerializer.Default);
            
            response.EnsureSuccessStatusCode();
            Assert.NotNull(content);
        }
        
        [Fact]
        public async Task GetDocumentById_Should_Succeed()
        {
            // Arrange   
            var document = await CreateDocument();
            
            // Act
            var response = await _client.GetAsync($"/api/documents/{document.Id}");
            
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync<DocumentWithPermissionsDto>(JsonSerializer.Default);

            Assert.NotNull(content);
        }

        private async Task<DocumentDto> CreateDocument()
        {
            // Arrange
            var document = _fixture.Create<CreateDocumentDto>();
            document.AuthorizedGroups = Array.Empty<int>();
            document.AuthorizedUsers = new []{ _user.Id };
            
            // Act
            var response = await _client.PostAsJsonAsync("/api/documents", document);
            response.EnsureSuccessStatusCode();
            
            var createdDocument = await response.Content.ReadFromJsonAsync<DocumentDto>(JsonSerializer.Default);

            return createdDocument!;
        }
        
        [Fact]
        public async Task AddUserPermission_Should_Succeed()
        {
            // Arrange   
            var document = await CreateDocument();
            var user = await _client.CreateUser(Role.Regular);
            
            // Act
            var response = await _client.PostAsJsonAsync($"/api/documents/{document.Id}/users/{user.Id}", new { });
            
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync<DocumentWithPermissionsDto>(JsonSerializer.Default);

            Assert.NotNull(content);
            await _client.DeleteUser(user.Id);
        }
        
        [Fact]
        public async Task AddGroupPermission_Should_Succeed()
        {
            // Arrange   
            var document = await CreateDocument();
            var group = await _client.CreateGroup();
            
            // Act
            var response = await _client.PostAsJsonAsync($"/api/documents/{document.Id}/groups/{group.Id}", new { });
            
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync<DocumentWithPermissionsDto>(JsonSerializer.Default);

            Assert.NotNull(content);
            await _client.DeleteGroup(group.Id);
        }
        
        [Fact]
        public async Task RemoveUserPermission_Should_Succeed()
        {
            // Arrange   
            var document = await CreateDocument();
            
            // Act
            var response = await _client.DeleteAsync($"/api/documents/{document.Id}/users/{_user.Id}");
            
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync<DocumentWithPermissionsDto>(JsonSerializer.Default);

            Assert.NotNull(content);
        }
        
        [Fact]
        public async Task RemoveGroupPermission_Should_Succeed()
        {
            // Arrange   
            var document = await CreateDocument();
            var group = await _client.CreateGroup();
            var createResponse = await _client.PostAsJsonAsync($"/api/documents/{document.Id}/groups/{group.Id}", new { });
            createResponse.EnsureSuccessStatusCode();

            
            // Act
            var response = await _client.DeleteAsync($"/api/documents/{document.Id}/groups/{group.Id}");
            
            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync<DocumentWithPermissionsDto>(JsonSerializer.Default);

            Assert.NotNull(content);
            await _client.DeleteGroup(group.Id);
        }
    }
}