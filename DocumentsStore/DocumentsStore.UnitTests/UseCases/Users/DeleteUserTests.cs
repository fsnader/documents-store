using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases;
using DocumentsStore.UseCases.Users;
using DocumentsStore.UseCases.Users.Abstractions;
using Moq;
using Moq.AutoMock;

namespace DocumentsStore.UnitTests.UseCases.Users;

public class DeleteUserTests
{
    private readonly AutoMocker _mocker;
    private readonly IDeleteUser _deleteUser;

    public DeleteUserTests()
    {
        _mocker = new AutoMocker();
        _deleteUser = _mocker.CreateInstance<DeleteUser>();
    }

    [Fact]
    public async Task ExecuteAsync_WithValidId_ReturnsSuccessResult()
    {
        // Arrange
        var id = 1;
        var deletedUser = new User { Id = id, Name = "Test User" };
        _mocker.GetMock<IUsersRepository>()
            .Setup(x => x.DeleteAsync(id, CancellationToken.None))
            .ReturnsAsync(deletedUser);

        // Act
        var result = await _deleteUser.ExecuteAsync(id, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(deletedUser, result.Result);
    }

    [Fact]
    public async Task ExecuteAsync_WithInvalidId_ReturnsBadRequestResult()
    {
        // Arrange
        var id = 0;

        // Act
        var result = await _deleteUser.ExecuteAsync(id, CancellationToken.None);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.BadRequest, result.Error);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistentUser_ReturnsNotFoundResult()
    {
        // Arrange
        var id = 1;
        _mocker.GetMock<IUsersRepository>()
            .Setup(x => x.DeleteAsync(id, CancellationToken.None))
            .ReturnsAsync((User)null);

        // Act
        var result = await _deleteUser.ExecuteAsync(id, CancellationToken.None);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.NotFound, result.Error);
    }
}