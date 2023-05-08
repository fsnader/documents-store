using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Groups.Abstractions;

public interface IDeleteGroup
{
    public Task<UseCaseResult<Group>> ExecuteAsync(int id, CancellationToken cancellationToken);
}