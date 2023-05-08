using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Users.Abstractions;

public interface IUpdateUser
{
    public Task<UseCaseResult<User>> ExecuteAsync(int id, User user, CancellationToken cancellationToken);
}