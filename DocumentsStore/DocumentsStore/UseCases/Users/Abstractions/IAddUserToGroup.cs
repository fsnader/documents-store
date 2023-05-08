using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Users.Abstractions;

public interface IAddUserToGroup
{
    public Task<UseCaseResult<User>> ExecuteAsync(int userId, int groupId, CancellationToken cancellationToken);
}