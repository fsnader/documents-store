using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Documents;
using Moq;
using Moq.AutoMock;

namespace DocumentsStore.UnitTests.UseCases.Documents;

public class GetUserAuthorizedDocumentsTests
{
    private readonly Mock<IDocumentsRepository> _documentsRepositoryMock;
    private readonly GetUserAuthorizedDocuments _getUserAuthorizedDocuments;

    public GetUserAuthorizedDocumentsTests()
    {
        var mocker = new AutoMocker();
        _documentsRepositoryMock = mocker.GetMock<IDocumentsRepository>();
        _getUserAuthorizedDocuments = mocker.CreateInstance<GetUserAuthorizedDocuments>();
    }

    [Fact]
    public async Task ExecuteAsync_WithValidUser_ReturnsSuccessResult()
    {
        // Arrange
        var user = new User { Id = 1 };
        var take = 10;
        var skip = 0;
        var documents = new List<Document>
        {
            new Document { Id = 1, UserId = 2, Name = "Document 1", Content = "Content 1" },
            new Document { Id = 2, UserId = 1, Name = "Document 2", Content = "Content 2" },
            new Document { Id = 3, UserId = 1, Name = "Document 3", Content = "Content 3" }
        };
        _documentsRepositoryMock
            .Setup(x => x.ListUserAuthorizedDocumentsAsync(user.Id, take, skip, default))
            .ReturnsAsync(documents);

        // Act
        var result = await _getUserAuthorizedDocuments.ExecuteAsync(user, take, skip, default);

        // Assert
        Assert.NotNull(result.Result);
        Assert.Equal(3, result.Result.Count());
    }
}