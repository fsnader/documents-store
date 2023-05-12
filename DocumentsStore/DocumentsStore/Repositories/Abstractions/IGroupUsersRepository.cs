using DocumentsStore.Domain;

namespace DocumentsStore.Repositories.Abstractions;

public interface IGroupUsersRepository
{
    public Task<IEnumerable<Group>> AddUserToGroupAsync(int userId, int groupId, CancellationToken cancellationToken);
    public Task<IEnumerable<Group>> RemoveUserFromGroupAsync(int userId, int groupId, CancellationToken cancellationToken);
    public Task<IEnumerable<Group>> GetGroupsByUserIdAsync(int userId, CancellationToken cancellationToken);
}