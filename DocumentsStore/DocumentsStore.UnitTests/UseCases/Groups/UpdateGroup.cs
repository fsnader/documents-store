using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases;
using DocumentsStore.UseCases.Groups;
using DocumentsStore.UseCases.Groups.Abstractions;
using Moq;
using Moq.AutoMock;

namespace DocumentsStore.UnitTests.UseCases.Groups;

public class UpdateGroupTests
{
    private readonly AutoMocker _mocker;
    private readonly IUpdateGroup _updateGroup;
    private readonly Mock<IGroupsRepository> _groupsRepository;

    public UpdateGroupTests()
    {
        _mocker = new AutoMocker();
        _updateGroup = _mocker.CreateInstance<UpdateGroup>();
        _groupsRepository = _mocker.GetMock<IGroupsRepository>();
    }

    [Fact]
    public async Task ExecuteAsync_WithValidInput_ReturnsSuccessResult()
    {
        // Arrange
        var id = 1;
        var group = new Group { Id = 1, Name = "Test Group" };
        var cancellationToken = CancellationToken.None;

        _groupsRepository.Setup(repo => repo
                .UpdateAsync(id, group, cancellationToken))
            .ReturnsAsync(group);

        // Act
        var result = await _updateGroup.ExecuteAsync(id, group, cancellationToken);

        // Assert
        Assert.NotNull(result.Result);
    }

    [Fact]
    public async Task ExecuteAsync_WithInvalidId_ReturnsBadRequestResult()
    {
        // Arrange
        var id = 0;
        var group = new Group { Id = 1, Name = "Test Group" };
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _updateGroup.ExecuteAsync(id, group, cancellationToken);

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
        var group = new Group { Id = 1, Name = "Test Group" };
        var cancellationToken = CancellationToken.None;

        _groupsRepository.Setup(repo => repo
                .UpdateAsync(id, group, cancellationToken))
            .ReturnsAsync((Group)null);

        // Act
        var result = await _updateGroup.ExecuteAsync(id, group, cancellationToken);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.NotFound, result.Error);
    }
}