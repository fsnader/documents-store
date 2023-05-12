using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Users;
using Moq;
using Moq.AutoMock;

namespace DocumentsStore.UnitTests.UseCases.Users;

public class GetAllUsersTests
{
    private readonly AutoMocker _mocker;
    private readonly GetAllUsers _getAllUsers;

    public GetAllUsersTests()
    {
        _mocker = new AutoMocker();
        _getAllUsers = _mocker.CreateInstance<GetAllUsers>();
    }

    [Fact]
    public async Task ExecuteAsync_WithValidParameters_ReturnsSuccessResult()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Name = "User 1" },
            new User { Id = 2, Name = "User 2" },
            new User { Id = 3, Name = "User 3" }
        };
        _mocker.GetMock<IUsersRepository>()
            .Setup(repo => repo.ListAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await _getAllUsers.ExecuteAsync(10, 0, CancellationToken.None);

        // Assert
        Assert.NotNull(result.Result);
        Assert.Equal(users.Count, result.Result.Count());
    }
}