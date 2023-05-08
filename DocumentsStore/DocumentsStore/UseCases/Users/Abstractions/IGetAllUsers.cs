using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Users.Abstractions;

public interface IGetAllUsers
{
    public Task<UseCaseResult<IEnumerable<User>>> ExecuteAsync(int take, int skip, CancellationToken cancellationToken);
}