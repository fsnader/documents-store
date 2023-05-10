using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Users.Abstractions;

public interface IGetUserGroups
{
    Task<UseCaseResult<IEnumerable<Group>>> ExecuteAsync(int id, CancellationToken cancellationToken);
}