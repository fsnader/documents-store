using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases;
using DocumentsStore.UseCases.Documents;
using Moq;
using Moq.AutoMock;

namespace DocumentsStore.UnitTests.UseCases.Documents;

public class GetDocumentByIdTests
{
    private readonly AutoMocker _mocker;
    
    private readonly GetDocumentById _getDocumentById;

    public GetDocumentByIdTests()
    {
        _mocker = new AutoMocker();
        _getDocumentById = _mocker.CreateInstance<GetDocumentById>();
    }

    [Fact]
    public async Task ExecuteAsync_WithInvalidDocumentId_ReturnsNotFound()
    {
        // Arrange
        var user = new User { Id = 1 };
        var cancellationToken = CancellationToken.None;
        
        _mocker.GetMock<IDocumentsRepository>()
            .Setup(repo => repo.GetDocumentByIdAsync(It.IsAny<int>(), cancellationToken))
            .ReturnsAsync(() => null);

        // Act
        var result = await _getDocumentById.ExecuteAsync(user, 123, cancellationToken);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.NotFound, result.Error);
    }

    [Fact]
    public async Task ExecuteAsync_WithoutPermission_ReturnsUnauthorized()
    {
        // Arrange
        var user = new User { Id = 1 };
        var cancellationToken = CancellationToken.None;
        var document = new Document { Id = 1, UserId = 2 };
        
        _mocker.GetMock<IDocumentsRepository>()
            .Setup(repo => repo.GetDocumentByIdAsync(document.Id, cancellationToken))
            .ReturnsAsync(() => document);
        
        _mocker.GetMock<IDocumentsRepository>()
            .Setup(repo => repo.CheckUserDocumentPermissionAsync(document.Id, user.Id, cancellationToken))
            .ReturnsAsync(() => false);

        // Act
        var result = await _getDocumentById.ExecuteAsync(user, document.Id, cancellationToken);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.Unauthorized, result.Error);
    }

    [Fact]
    public async Task ExecuteAsync_WithPermission_ReturnsDocument()
    {
        // Arrange
        var user = new User { Id = 1 };
        var cancellationToken = CancellationToken.None;
        var document = new Document { Id = 1, UserId = user.Id };
        
        _mocker.GetMock<IDocumentsRepository>()
            .Setup(repo => repo.GetDocumentByIdAsync(document.Id, cancellationToken))
            .ReturnsAsync(() => document);
        
        _mocker.GetMock<IDocumentsRepository>()
            .Setup(repo => repo.CheckUserDocumentPermissionAsync(document.Id, user.Id, cancellationToken))
            .ReturnsAsync(() => true);

        // Act
        var result = await _getDocumentById.ExecuteAsync(user, document.Id, cancellationToken);

        // Assert
        Assert.Equal(document, result.Result);
    }
}