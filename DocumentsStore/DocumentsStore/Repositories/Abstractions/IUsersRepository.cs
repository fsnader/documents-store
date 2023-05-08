using DocumentsStore.Domain;

namespace DocumentsStore.Repositories.Abstractions;

public interface IUsersRepository
{
    public Task<User> CreateAsync(User user, CancellationToken cancellationToken);
}