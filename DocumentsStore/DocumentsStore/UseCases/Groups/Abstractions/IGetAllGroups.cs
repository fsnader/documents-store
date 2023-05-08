using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Groups.Abstractions;

public interface IGetAllGroups
{
    public Task<UseCaseResult<IEnumerable<Group>>> ExecuteAsync(int take, int skip, CancellationToken cancellationToken);
}