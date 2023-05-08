using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Users;

public interface IGetAllUsers
{
    public Task<UseCaseResult<IEnumerable<User>>> ExecuteAsync(CancellationToken cancellationToken);
}