using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases;
using DocumentsStore.UseCases.Groups;
using DocumentsStore.UseCases.Groups.Abstractions;
using Moq;
using Moq.AutoMock;

namespace DocumentsStore.UnitTests.UseCases.Groups;

public class DeleteGroupTests
{
    private readonly AutoMocker _mocker;
    private readonly IDeleteGroup _deleteGroup;

    public DeleteGroupTests()
    {
        _mocker = new AutoMocker();
        _deleteGroup = _mocker.CreateInstance<DeleteGroup>();
    }

    [Fact]
    public async Task ExecuteAsync_WithValidId_ReturnsSuccessResult()
    {
        // Arrange
        var id = 1;
        var deletedGroup = new Group { Id = id, Name = "Test Group" };
        _mocker.GetMock<IGroupsRepository>()
            .Setup(x => x.DeleteAsync(id, CancellationToken.None))
            .ReturnsAsync(deletedGroup);

        // Act
        var result = await _deleteGroup.ExecuteAsync(id, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(deletedGroup, result.Result);
    }

    [Fact]
    public async Task ExecuteAsync_WithInvalidId_ReturnsBadRequestResult()
    {
        // Arrange
        var id = 0;

        // Act
        var result = await _deleteGroup.ExecuteAsync(id, CancellationToken.None);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.BadRequest, result.Error);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistentGroup_ReturnsNotFoundResult()
    {
        // Arrange
        var id = 1;
        _mocker.GetMock<IGroupsRepository>()
            .Setup(x => x.DeleteAsync(id, CancellationToken.None))
            .ReturnsAsync((Group)null);

        // Act
        var result = await _deleteGroup.ExecuteAsync(id, CancellationToken.None);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.NotFound, result.Error);
    }
}