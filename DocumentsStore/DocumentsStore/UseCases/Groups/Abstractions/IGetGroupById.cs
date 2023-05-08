using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Groups.Abstractions;

public interface IGetGroupById
{
    public Task<UseCaseResult<Group>> ExecuteAsync(int id, CancellationToken cancellationToken);
}