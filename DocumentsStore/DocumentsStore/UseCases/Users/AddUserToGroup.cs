using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.Repositories.Exceptions;
using DocumentsStore.UseCases.Users.Abstractions;

namespace DocumentsStore.UseCases.Users;

public class AddUserToGroup : IAddUserToGroup
{
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IGroupsRepository _groupsRepository;
    private readonly IUsersRepository _usersRepository;

    public AddUserToGroup(IGroupUsersRepository groupUsersRepository, IGroupsRepository groupsRepository, IUsersRepository usersRepository)
    {
        _groupUsersRepository = groupUsersRepository;
        _groupsRepository = groupsRepository;
        _usersRepository = usersRepository;
    }
    
    public async Task<UseCaseResult<User>> ExecuteAsync(int userId, int groupId, CancellationToken cancellationToken)
    {
        var userTask = _usersRepository.GetByIdAsync(userId, cancellationToken);
        var group = _groupsRepository.GetByIdAsync(groupId, cancellationToken);

        await Task.WhenAll(userTask, group);
        var user = await userTask;
        
        if (user is null)
        {
            return UseCaseResult<User>.NotFound("User not found");
        }

        if (await group is null)
        {
            return UseCaseResult<User>.NotFound("Group not found");
        }

        try
        {
            user.Groups = await _groupUsersRepository.AddUserToGroupAsync(userId, groupId, cancellationToken);
            return UseCaseResult<User>.Success(user);
        }
        catch (UniqueException)
        {
            return UseCaseResult<User>.BadRequest("This user is already a group member");
        }
    }
}