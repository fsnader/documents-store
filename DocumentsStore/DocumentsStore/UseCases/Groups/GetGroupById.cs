using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Groups.Abstractions;

namespace DocumentsStore.UseCases.Groups;

public class GetGroupById : IGetGroupById
{
    private readonly IGroupsRepository _groupsRepository;

    public GetGroupById(IGroupsRepository groupsRepository)
    {
        _groupsRepository = groupsRepository;
    }
    
    public async Task<UseCaseResult<Group>> ExecuteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return UseCaseResult<Group>.BadRequest("Please provide a valid id");
        }

        var group = await _groupsRepository.GetByIdAsync(id, cancellationToken);

        if (group is null)
        {
            return UseCaseResult<Group>.NotFound();
        }

        return UseCaseResult<Group>.Success(group);
    }
}