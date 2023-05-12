using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.Repositories.Exceptions;
using DocumentsStore.UseCases;
using DocumentsStore.UseCases.Users;
using DocumentsStore.UseCases.Users.Abstractions;
using Moq;
using Moq.AutoMock;

namespace DocumentsStore.UnitTests.UseCases.Users;

public class CreateUserTests
{
    private readonly Mock<IUsersRepository> _mockUsersRepository;
    private readonly ICreateUser _createUser;

    public CreateUserTests()
    {
        var autoMocker = new AutoMocker();
        _mockUsersRepository = autoMocker.GetMock<IUsersRepository>();
        _createUser = autoMocker.CreateInstance<CreateUser>();
    }

    [Fact]
    public async Task ExecuteAsync_InputIsValid_ReturnsSuccessResult()
    {
        // Arrange
        var user = new User { Name = "Test User", Email = "teste@test.com"};
        var cancellationToken = CancellationToken.None;
        _mockUsersRepository.Setup(x => x.CreateAsync(user, cancellationToken)).ReturnsAsync(user);

        // Act
        var result = await _createUser.ExecuteAsync(user, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user, result.Result);
    }

    [Fact]
    public async Task ExecuteAsync_InputIsNotValid_ReturnsBadRequestResult()
    {
        // Arrange
        var user = new User { Name = null };
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _createUser.ExecuteAsync(user, cancellationToken);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.BadRequest, result.Error);
    }
    
    [Fact]
    public async Task ExecuteAsync_DuplicateEmail_ReturnsBadRequest()
    {
        // Arrange
        var user = new User { Name = "Test User", Email = "teste@test.com"};
        var cancellationToken = CancellationToken.None;
        _mockUsersRepository.Setup(x => x.CreateAsync(user, cancellationToken)).ThrowsAsync(new UniqueException());

        // Act
        var result = await _createUser.ExecuteAsync(user, cancellationToken);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.BadRequest, result.Error);
    }
}