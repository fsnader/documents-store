using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Groups.Abstractions;

public interface ICreateGroup
{
    public Task<UseCaseResult<Group>> ExecuteAsync(Group group, CancellationToken cancellationToken);
}