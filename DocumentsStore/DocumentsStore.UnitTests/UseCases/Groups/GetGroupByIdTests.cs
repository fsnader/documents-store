using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases;
using DocumentsStore.UseCases.Groups;
using Moq;
using Moq.AutoMock;

namespace DocumentsStore.UnitTests.UseCases.Groups; 

public class GetGroupByIdTests
{
    private readonly Mock<IGroupsRepository> _groupsRepository;
    private readonly GetGroupById _getGroupById;
    public GetGroupByIdTests()
    {
        var mocker = new AutoMocker();
        _groupsRepository = mocker.GetMock<IGroupsRepository>();
        _getGroupById = mocker.CreateInstance<GetGroupById>();

    }
    
    [Fact]
    public async Task ExecuteAsync_WithValidId_ReturnsGroup()
    {
        // Arrange
        var groupId = 1;
        var expectedGroup = new Group { Id = groupId, Name = "Group 1" };
        
        _groupsRepository.Setup(repo => repo.GetByIdAsync(groupId, CancellationToken.None))
            .ReturnsAsync(expectedGroup);

        // Act
        var result = await _getGroupById.ExecuteAsync(groupId, CancellationToken.None);

        // Assert
        Assert.NotNull(result.Result);
    }

    [Fact]
    public async Task ExecuteAsync_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        var invalidGroupId = 0;
        
        // Act
        var result = await _getGroupById.ExecuteAsync(invalidGroupId, CancellationToken.None);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.BadRequest, result.Error);
        Assert.Equal("Please provide a valid id", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonexistentId_ReturnsNotFound()
    {
        // Arrange
        var groupId = 1;
        _groupsRepository.Setup(repo => repo.GetByIdAsync(groupId, CancellationToken.None))
            .ReturnsAsync((Group)null);
        
        // Act
        var result = await _getGroupById.ExecuteAsync(groupId, CancellationToken.None);

        // Assert
        Assert.Null(result.Result);
        Assert.Equal(ErrorType.NotFound, result.Error);
    }
}