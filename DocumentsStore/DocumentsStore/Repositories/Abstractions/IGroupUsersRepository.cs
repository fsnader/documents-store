using DocumentsStore.Domain;

namespace DocumentsStore.Repositories.Abstractions;

public interface IGroupUsersRepository
{
    public Task<IEnumerable<Group>> AddUserToGroup(int userId, int groupId, CancellationToken cancellationToken);
    public Task<IEnumerable<Group>> RemoveUserFromGroup(int userId, int groupId, CancellationToken cancellationToken);
    public Task<IEnumerable<Group>> GetGroupsByUserIdAsync(int userId, CancellationToken cancellationToken);
}