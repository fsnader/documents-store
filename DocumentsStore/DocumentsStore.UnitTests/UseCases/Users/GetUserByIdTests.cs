using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases;
using DocumentsStore.UseCases.Users;
using Moq;
using Moq.AutoMock;

namespace DocumentsStore.UnitTests.UseCases.Users; 

public class GetUserByIdTests
{
    private readonly Mock<IUsersRepository> _usersRepository;
    private readonly GetUserById _getUserById;
    public GetUserByIdTests()
    {
        var mocker = new AutoMocker();
        _usersRepository = mocker.GetMock<IUsersRepository>();
        _getUserById = mocker.CreateInstance<GetUserById>();

    }
    
    [Fact]
    public async Task ExecuteAsync_WithValidId_ReturnsUser()
    {
        // Arrange
        var userId = 1;
        var expectedUser = new User { Id = userId, Name = "User 1" };
        
        _usersRepository.Setup(repo => repo.GetByIdAsync(userId, CancellationToken.None))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _getUserById.ExecuteAsync(userId, CancellationToken.None);

        // Assert
        Assert.NotNull(result.Result);
    }

    [Fact]
    public async Task ExecuteAsync_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        var invalidUserId = 0;
        
        // Act
        var result = await _getUserById.ExecuteAsync(invalidUserId, CancellationToken.None);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.BadRequest, result.Error);
        Assert.Equal("Please provide a valid id", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonexistentId_ReturnsNotFound()
    {
        // Arrange
        var userId = 1;
        _usersRepository.Setup(repo => repo.GetByIdAsync(userId, CancellationToken.None))
            .ReturnsAsync((User)null);
        
        // Act
        var result = await _getUserById.ExecuteAsync(userId, CancellationToken.None);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.NotFound, result.Error);
    }
}