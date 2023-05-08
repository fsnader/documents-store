using DocumentsStore.Domain;

namespace DocumentsStore.Repositories.Abstractions;

public interface IGroupUsersRepository
{
    public Task<User> AddUserToGroup(int userId, int groupId, CancellationToken cancellationToken);
    public Task<User> RemoveUserToGroup(int userId, int groupId, CancellationToken cancellationToken);
}