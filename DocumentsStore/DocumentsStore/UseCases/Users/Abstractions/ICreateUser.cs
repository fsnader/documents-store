using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Users.Abstractions;

public interface ICreateUser
{
    public Task<UseCaseResult<User>> ExecuteAsync(User user, CancellationToken cancellationToken);
}