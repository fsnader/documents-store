using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Users.Abstractions;

namespace DocumentsStore.UseCases.Users;

public class RemoveUserFromGroup : IRemoveUserFromGroup
{
    private readonly IGroupUsersRepository _groupUsersRepository;

    public RemoveUserFromGroup(IGroupUsersRepository groupUsersRepository)
    {
        _groupUsersRepository = groupUsersRepository;
    }
    
    public async Task<UseCaseResult<User>> ExecuteAsync(int userId, int groupId, CancellationToken cancellationToken)
    {
        var result = await _groupUsersRepository.RemoveUserFromGroup(userId, groupId, cancellationToken);

        return UseCaseResult<User>.Success(result);
    }
}