using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.Repositories.Exceptions;
using DocumentsStore.UseCases;
using DocumentsStore.UseCases.Documents;
using Moq;
using Moq.AutoMock;

namespace DocumentsStore.UnitTests.UseCases.Documents;

public class RemoveDocumentPermissionTests
{
    private readonly AutoMocker _mocker;
    private readonly RemoveDocumentPermission _removeDocumentPermission;

    public RemoveDocumentPermissionTests()
    {
        _mocker = new AutoMocker();
        _removeDocumentPermission = _mocker.CreateInstance<RemoveDocumentPermission>();
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsNotFound_WhenDocumentDoesNotExist()
    {
        // Arrange
        var user = new User { Id = 1 };
        var documentId = 1;
        var permissionId = 1;
        var permissionType = PermissionType.User;
        var cancellationToken = CancellationToken.None;

        _mocker.GetMock<IDocumentsRepository>()
            .Setup(x => x.GetDocumentByIdAsync(documentId, cancellationToken))
            .ReturnsAsync((Document)null);

        // Act
        var result = await _removeDocumentPermission.ExecuteAsync(user, documentId, permissionId, permissionType, cancellationToken);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.NotFound, result.Error);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsUnauthorized_WhenUserDoesNotOwnDocument()
    {
        // Arrange
        var user = new User { Id = 1 };
        var document = new Document { UserId = 2 };
        var documentId = 1;
        var permissionId = 1;
        var permissionType = PermissionType.User;
        var cancellationToken = CancellationToken.None;

        _mocker.GetMock<IDocumentsRepository>()
            .Setup(x => x.GetDocumentByIdAsync(documentId, cancellationToken))
            .ReturnsAsync(document);

        // Act
        var result = await _removeDocumentPermission.ExecuteAsync(user, documentId, permissionId, permissionType, cancellationToken);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.Unauthorized, result.Error);
    }

    [Theory]
    [InlineData(PermissionType.Group)]
    [InlineData(PermissionType.User)]
    public async Task ExecuteAsync_ReturnsSuccess_WhenPermissionIsRemoved(PermissionType permissionType)
    {
        // Arrange
        var user = new User { Id = 1 };
        var document = new Document { UserId = user.Id };
        var documentId = 1;
        var permissionId = 1;
        var cancellationToken = CancellationToken.None;

        _mocker.GetMock<IDocumentsRepository>()
            .Setup(x => x.GetDocumentByIdAsync(documentId, cancellationToken))
            .ReturnsAsync(document);

        _mocker.GetMock<IDocumentsRepository>()
            .Setup(x => x.RemoveDocumentGroupsPermissionAsync(documentId, permissionId, cancellationToken))
            .Returns(Task.CompletedTask);

        _mocker.GetMock<IDocumentsRepository>()
            .Setup(x => x.GetDocumentUsersPermissionsAsync(documentId, cancellationToken))
            .ReturnsAsync(new List<int>());

        _mocker.GetMock<IDocumentsRepository>()
            .Setup(x => x.GetDocumentGroupsPermissionsAsync(documentId, cancellationToken))
            .ReturnsAsync(new List<int>());

        // Act
        var result = await _removeDocumentPermission.ExecuteAsync(user, documentId, permissionId, permissionType, cancellationToken);

        // Assert
        Assert.Equal(document, result.Result);
    }
}