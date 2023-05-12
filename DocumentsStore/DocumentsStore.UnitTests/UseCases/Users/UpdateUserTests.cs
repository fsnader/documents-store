using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.Repositories.Exceptions;
using DocumentsStore.UseCases;
using DocumentsStore.UseCases.Users;
using DocumentsStore.UseCases.Users.Abstractions;
using Moq;
using Moq.AutoMock;

namespace DocumentsStore.UnitTests.UseCases.Users;

public class UpdateUserTests
{
    private readonly AutoMocker _mocker;
    private readonly IUpdateUser _updateUser;
    private readonly Mock<IUsersRepository> _usersRepository;

    public UpdateUserTests()
    {
        _mocker = new AutoMocker();
        _updateUser = _mocker.CreateInstance<UpdateUser>();
        _usersRepository = _mocker.GetMock<IUsersRepository>();
    }

    [Fact]
    public async Task ExecuteAsync_WithValidInput_ReturnsSuccessResult()
    {
        // Arrange
        var id = 1;
        var user = new User { Id = 1, Name = "Test User" };
        var cancellationToken = CancellationToken.None;

        _usersRepository.Setup(repo => repo
                .UpdateAsync(id, user, cancellationToken))
            .ReturnsAsync(user);

        // Act
        var result = await _updateUser.ExecuteAsync(id, user, cancellationToken);

        // Assert
        Assert.NotNull(result.Result);
    }

    [Fact]
    public async Task ExecuteAsync_WithInvalidId_ReturnsBadRequestResult()
    {
        // Arrange
        var id = 0;
        var user = new User { Id = 1, Name = "Test User" };
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _updateUser.ExecuteAsync(id, user, cancellationToken);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.BadRequest, result.Error);
        Assert.Equal("Please provide a valid id", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_WithNotFound_ReturnsNotFoundResult()
    {
        // Arrange
        var id = 1;
        var user = new User { Id = 1, Name = "Test User" };
        var cancellationToken = CancellationToken.None;

        _usersRepository.Setup(repo => repo
                .UpdateAsync(id, user, cancellationToken))
            .ReturnsAsync((User)null);

        // Act
        var result = await _updateUser.ExecuteAsync(id, user, cancellationToken);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.NotFound, result.Error);
    }
    
    [Fact]
    public async Task ExecuteAsync_DuplicateEmail_ReturnsBadRequest()
    {
        // Arrange
        var id = 1;
        var user = new User { Id = 1, Name = "Test User" };
        var cancellationToken = CancellationToken.None;

        _usersRepository.Setup(repo => repo
                .UpdateAsync(id, user, cancellationToken))
            .ThrowsAsync(new UniqueException());

        // Act
        var result = await _updateUser.ExecuteAsync(id, user, cancellationToken);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.BadRequest, result.Error);
    }
}