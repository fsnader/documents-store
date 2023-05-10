using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Users.Abstractions;

namespace DocumentsStore.UseCases.Users;

public class RemoveUserFromGroup : IRemoveUserFromGroup
{
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IGroupsRepository _groupsRepository;
    private readonly IUsersRepository _usersRepository;

    public RemoveUserFromGroup(IGroupUsersRepository groupUsersRepository, IGroupsRepository groupsRepository, IUsersRepository usersRepository)
    {
        _groupUsersRepository = groupUsersRepository;
        _groupsRepository = groupsRepository;
        _usersRepository = usersRepository;
    }
    
    public async Task<UseCaseResult<User>> ExecuteAsync(int userId, int groupId, CancellationToken cancellationToken)
    {
        var user = _usersRepository.GetByIdAsync(userId, cancellationToken);
        var group = _groupsRepository.GetByIdAsync(groupId, cancellationToken);

        await Task.WhenAll(user, group);
        
        if (await user is null)
        {
            return UseCaseResult<User>.NotFound("User not found");
        }

        if (await group is null)
        {
            return UseCaseResult<User>.NotFound("Group not found");
        }
        
        await _groupUsersRepository.RemoveUserFromGroup(userId, groupId, cancellationToken);

        return UseCaseResult<User>.Success((await user)!);
    }
}