using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Groups.Abstractions;

public interface IUpdateGroup
{
    public Task<UseCaseResult<Group>> ExecuteAsync(int id, Group group, CancellationToken cancellationToken);
}