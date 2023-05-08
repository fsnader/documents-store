using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Groups.Abstractions;

namespace DocumentsStore.UseCases.Groups;

public class CreateGroup : ICreateGroup
{
    private readonly IGroupsRepository _groupsRepository;

    public CreateGroup(IGroupsRepository groupsRepository) => _groupsRepository = groupsRepository;

    public async Task<UseCaseResult<Group>> ExecuteAsync(Group group, CancellationToken cancellationToken)
    {
        if (!IsInputValid(group))
        {
            return UseCaseResult<Group>.BadRequest();
        }

        var created = await _groupsRepository.CreateAsync(group, cancellationToken);
        
        return UseCaseResult<Group>.Success(created);
    }
    
    private bool IsInputValid(Group group) =>
        !string.IsNullOrWhiteSpace(group.Name);
}