using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases;
using DocumentsStore.UseCases.Groups;
using DocumentsStore.UseCases.Groups.Abstractions;
using Moq;
using Moq.AutoMock;

namespace DocumentsStore.UnitTests.UseCases.Groups;

public class CreateGroupTests
{
    private readonly Mock<IGroupsRepository> _mockGroupsRepository;
    private readonly ICreateGroup _createGroup;

    public CreateGroupTests()
    {
        var autoMocker = new AutoMocker();
        _mockGroupsRepository = autoMocker.GetMock<IGroupsRepository>();
        _createGroup = autoMocker.CreateInstance<CreateGroup>();
    }

    [Fact]
    public async Task ExecuteAsync_InputIsValid_ReturnsSuccessResult()
    {
        // Arrange
        var group = new Group { Name = "Test Group" };
        var cancellationToken = CancellationToken.None;
        _mockGroupsRepository.Setup(x => x.CreateAsync(group, cancellationToken)).ReturnsAsync(group);

        // Act
        var result = await _createGroup.ExecuteAsync(group, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(group, result.Result);
    }

    [Fact]
    public async Task ExecuteAsync_InputIsNotValid_ReturnsBadRequestResult()
    {
        // Arrange
        var group = new Group { Name = null };
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _createGroup.ExecuteAsync(group, cancellationToken);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.BadRequest, result.Error);
    }
}