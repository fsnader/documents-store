using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;

namespace DocumentsStore.Repositories;

public class GroupUsersRepository : IGroupUsersRepository
{
    public Task<User> AddUserToGroup(int userId, int groupId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<User> RemoveUserToGroup(int userId, int groupId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}