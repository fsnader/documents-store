using DocumentsStore.Domain;

namespace DocumentsStore.Repositories.Abstractions;

public interface IUsersRepository
{
    Task<IEnumerable<User>> ListAllAsync(int take, int skip, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
    public Task<User> CreateAsync(User user, CancellationToken cancellationToken);
    public Task<User?> UpdateAsync(int id, User user, CancellationToken cancellationToken);
    public Task<User?> DeleteAsync(int id, CancellationToken cancellationToken);

}