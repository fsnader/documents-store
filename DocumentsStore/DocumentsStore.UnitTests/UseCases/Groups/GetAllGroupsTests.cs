using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Groups;
using Moq;
using Moq.AutoMock;

namespace DocumentsStore.UnitTests.UseCases.Groups;

public class GetAllGroupsTests
{
    private readonly AutoMocker _mocker;
    private readonly GetAllGroups _getAllGroups;

    public GetAllGroupsTests()
    {
        _mocker = new AutoMocker();
        _getAllGroups = _mocker.CreateInstance<GetAllGroups>();
    }

    [Fact]
    public async Task ExecuteAsync_WithValidParameters_ReturnsSuccessResult()
    {
        // Arrange
        var groups = new List<Group>
        {
            new Group { Id = 1, Name = "Group 1" },
            new Group { Id = 2, Name = "Group 2" },
            new Group { Id = 3, Name = "Group 3" }
        };
        _mocker.GetMock<IGroupsRepository>()
            .Setup(repo => repo.ListAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groups);

        // Act
        var result = await _getAllGroups.ExecuteAsync(10, 0, CancellationToken.None);

        // Assert
        Assert.NotNull(result.Result);
        Assert.Equal(groups.Count, result.Result.Count());
    }
}