using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases;
using DocumentsStore.UseCases.Documents;
using Moq;
using Moq.AutoMock;

namespace DocumentsStore.UnitTests.UseCases.Documents;

public class CreateDocumentTests
{
    private readonly AutoMocker _mocker;
    private readonly CreateDocument _createDocument;

    public CreateDocumentTests()
    {
        _mocker = new AutoMocker();
        _createDocument = _mocker.CreateInstance<CreateDocument>();
    }

    [Fact]
    public async Task ExecuteAsync_UserIsNotAdminOrManager_ReturnsUnauthorized()
    {
        // Arrange
        var user = new User { Role = Role.Regular };
        var document = new Document { Name = "Test", Description = "Description", Content = "Lorem"};;
        var users = new int[] { 1, 2 };
        var groups = new int[] { 3, 4 };
        var cancellationToken = new CancellationToken();

        // Act
        var result = await _createDocument.ExecuteAsync(user, document, users, groups, cancellationToken);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.Unauthorized, result.Error);
        Assert.Equal("User doesn't have permission to perform this action", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_DocumentIsNotValid_ReturnsBadRequest()
    {
        // Arrange
        var user = new User { Role = Role.Admin };
        var document = new Document { Name = "Test" };
        var users = new int[] { 1, 2 };
        var groups = new int[] { 3, 4 };
        var cancellationToken = new CancellationToken();

        // Act
        var result = await _createDocument.ExecuteAsync(user, document, users, groups, cancellationToken);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.BadRequest, result.Error);
        Assert.Equal("Please provide all document required fields", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_NoUsersOrGroupsProvided_ReturnsBadRequest()
    {
        // Arrange
        var user = new User { Role = Role.Admin };
        var document = new Document
        {
            Name = "Test", Description = "Test", Category = Category.General, Content = "Test",
            PostedDate = DateTime.Now
        };
        var users = new int[] { };
        var groups = new int[] { };
        var cancellationToken = new CancellationToken();

        // Act
        var result = await _createDocument.ExecuteAsync(user, document, users, groups, cancellationToken);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.BadRequest, result.Error);
        Assert.Equal("Please provide a list of user or group permissions", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_ValidInput_ReturnsSuccessResult()
    {
        // Arrange
        var user = new User { Id = 1, Role = Role.Admin };
        var document = new Document
        {
            Name = "Test", Description = "Test", Category = Category.General, Content = "Test",
            PostedDate = DateTime.Now
        };
        
        var users = new[] { 2, 3 };
        var groups = new[] { 4, 5 };
        var cancellationToken = new CancellationToken();

        var createdDocument = new Document
        {
            Id = 1, 
            Name = document.Name, 
            Description = document.Description, 
            Category = document.Category,
            Content = document.Content, 
            PostedDate = document.PostedDate, 
            UserId = user.Id
        };
        
        _mocker.GetMock<IDocumentsRepository>()
            .Setup(x => x.CreateDocumentAsync(document, users, groups, cancellationToken))
            .ReturnsAsync(createdDocument);

        // Act
        var result = await _createDocument.ExecuteAsync(user, document, users, groups, cancellationToken);

        // Assert
        Assert.NotNull(result.Result);
        Assert.Equal(createdDocument, result.Result);
    }
}