using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Users.Abstractions;

public interface IDeleteUser
{
    public Task<UseCaseResult<User>> ExecuteAsync(int id, CancellationToken cancellationToken);
}