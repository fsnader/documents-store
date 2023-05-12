using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using DocumentsStore.Api.DTOs.Users;
using DocumentsStore.Domain;
using DocumentsStore.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DocumentsStore.IntegrationTests.Authentication
{
    public class AuthTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _testFixtureBase;
        private readonly HttpClient _client;
        private readonly Fixture _fixture;

        public AuthTests(WebApplicationFactory<Program> testFixtureBase)
        {
            _testFixtureBase = testFixtureBase;
            _fixture = new Fixture();
            _client = _testFixtureBase.CreateDefaultClient();
        }

        [Fact]
        public async Task LoginUser()
        {
            // Arrange and Act   
            var response = await _client.PostAsJsonAsync("/api/auth/login", new { Id = 1 });
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.False(string.IsNullOrWhiteSpace(content));
        }
        
        [Fact]
        public async Task LoginUser_WithInvalidId_ReturnsUnauthorized()
        {
            // Arrange and Act   
            var response = await _client.PostAsJsonAsync("/api/auth/login", new { Id = -1 });
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.False(string.IsNullOrWhiteSpace(content));
        }

        [Fact]
        public async Task SignupUser()
        {
            // Arrange
            var payload = new
            {
                Name = _fixture.Create<string>(),
                Email = _fixture.Create<string>(),
                Role = _fixture.Create<Role>(),
            };
            
            // Arrange and Act   
            var response = await _client.PostAsJsonAsync("/api/auth/signup", payload);
            var body = await response.Content.ReadFromJsonAsync<UserDto>(JsonSerializer.Default);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(body);
            Assert.Equal(payload.Name, body.Name);

            await _client.DeleteUser(body.Id);
        }
    }
}