using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;

namespace DocumentsStore.Repositories;

public class GroupsRepository : IGroupsRepository
{
    public Task<Group?> CreateAsync(Group group, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Group?> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Group>> ListAllAsync(int take, int skip, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Group?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Group?> UpdateAsync(int id, Group group, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}