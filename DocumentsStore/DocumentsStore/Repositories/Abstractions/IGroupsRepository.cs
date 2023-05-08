using DocumentsStore.Domain;

namespace DocumentsStore.Repositories.Abstractions;

public interface IGroupsRepository
{
    Task<Group?> CreateAsync(Group group, CancellationToken cancellationToken);
    Task<Group?> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Group>> ListAllAsync(int take, int skip, CancellationToken cancellationToken);
    Task<Group?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Group?> UpdateAsync(int id, Group group, CancellationToken cancellationToken);
}