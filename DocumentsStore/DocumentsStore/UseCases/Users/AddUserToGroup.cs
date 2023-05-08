using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Users.Abstractions;

namespace DocumentsStore.UseCases.Users;

public class AddUserToGroup : IAddUserToGroup
{
    private readonly IGroupUsersRepository _groupUsersRepository;

    public AddUserToGroup(IGroupUsersRepository groupUsersRepository)
    {
        _groupUsersRepository = groupUsersRepository;
    }
    
    public async Task<UseCaseResult<User>> ExecuteAsync(int userId, int groupId, CancellationToken cancellationToken)
    {
        var user = await _groupUsersRepository.AddUserToGroup(userId, groupId, cancellationToken);
        return UseCaseResult<User>.Success(user);
    }
}