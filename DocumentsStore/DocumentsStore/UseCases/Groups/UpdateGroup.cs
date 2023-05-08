using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Groups.Abstractions;

namespace DocumentsStore.UseCases.Groups;

public class UpdateGroup : IUpdateGroup
{
    private readonly IGroupsRepository _groupsRepository;

    public UpdateGroup(IGroupsRepository groupsRepository)
    {
        _groupsRepository = groupsRepository;
    }
    
    public async Task<UseCaseResult<Group>> ExecuteAsync(int id, Group group, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return UseCaseResult<Group>.BadRequest("Please provide a valid id");
        }

        var updated = await _groupsRepository.UpdateAsync(id, group, cancellationToken);

        if (updated is null)
        {
            return UseCaseResult<Group>.NotFound();
        }
        
        return UseCaseResult<Group>.Success(updated);
    }
}