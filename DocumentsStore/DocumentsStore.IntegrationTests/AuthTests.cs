using System.Net.Http.Json;
using AutoFixture;
using DocumentsStore.Domain;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DocumentsStore.IntegrationTests
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
          
            // Invoke the app
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
            
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains(payload.Name, content);
        }
    }
}