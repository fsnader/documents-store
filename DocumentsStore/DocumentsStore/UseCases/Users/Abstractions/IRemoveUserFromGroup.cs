using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Users.Abstractions;

public interface IRemoveUserFromGroup
{
    public Task<UseCaseResult<User>> ExecuteAsync(int userId, int groupId, CancellationToken cancellationToken);
}