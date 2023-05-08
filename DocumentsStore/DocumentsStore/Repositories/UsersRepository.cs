using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;

namespace DocumentsStore.Repositories;

public class UsersRepository : IUsersRepository
{
    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken)
    {
        return user;
    }
}