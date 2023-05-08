using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Users;

public interface IGetUserById
{
    public Task<UseCaseResult<User>> ExecuteAsync(int id, CancellationToken cancellationToken);
}