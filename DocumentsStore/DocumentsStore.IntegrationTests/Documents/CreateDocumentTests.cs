using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using DocumentsStore.Api.DTOs.Documents;
using DocumentsStore.Domain;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DocumentsStore.IntegrationTests.Documents
{
    public class CreateDocumentTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _testFixtureBase;
        private readonly HttpClient _client;
        private readonly Fixture _fixture;

        public CreateDocumentTests(WebApplicationFactory<Program> testFixtureBase)
        {
            _testFixtureBase = testFixtureBase;
            _fixture = new Fixture();
            _client = _testFixtureBase.CreateDefaultClient();
        }

        [Fact]
        public async Task CreateDocument_Should_Succeed()
        {
            // Arrange and Act   
            var user = await _client.CreateAndLoginUser(Role.Admin);
            var document = _fixture.Create<CreateDocumentDto>();
            document.AuthorizedGroups = Array.Empty<int>();
            document.AuthorizedUsers = new []{ user.Id };
            
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
            var user = await _client.CreateAndLoginUser(Role.Admin);
            var document = _fixture.Create<CreateDocumentDto>();
            document.AuthorizedGroups = Array.Empty<int>();
            document.AuthorizedUsers = Array.Empty<int>();
            
            // Act
            var response = await _client.PostAsJsonAsync("/api/documents", document);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}