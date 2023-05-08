using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Groups.Abstractions;

namespace DocumentsStore.UseCases.Groups;

public class DeleteGroup : IDeleteGroup
{
    private readonly IGroupsRepository _groupsRepository;

    public DeleteGroup(IGroupsRepository groupsRepository)
    {
        _groupsRepository = groupsRepository;
    }
    
    public async Task<UseCaseResult<Group>> ExecuteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return UseCaseResult<Group>.BadRequest("Please provide a valid id");
        }
        
        var deleted = await _groupsRepository.DeleteAsync(id, cancellationToken);

        if (deleted is null)
        {
            return UseCaseResult<Group>.NotFound();
        }
        
        return UseCaseResult<Group>.Success(deleted);
        
    }
}