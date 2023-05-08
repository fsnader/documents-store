using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Groups.Abstractions;

namespace DocumentsStore.UseCases.Groups;

public class GetAllGroups : IGetAllGroups
{
    private readonly IGroupsRepository _groupsRepository;

    public GetAllGroups(IGroupsRepository groupsRepository) => _groupsRepository = groupsRepository;

    public async Task<UseCaseResult<IEnumerable<Group>>> ExecuteAsync(int take, int skip, CancellationToken cancellationToken)
    {
        var results = await _groupsRepository.ListAllAsync(take, skip, cancellationToken);

        return UseCaseResult<IEnumerable<Group>>.Success(results);
    }
}