using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;

namespace DocumentsStore.Repositories;

public class UsersRepository : IUsersRepository
{
    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return new()
        {
            Id = 0,
            Name = "aaa",
            Email = "",
            Role = Role.Regular,
            Groups = null
        };
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken)
    {
        return user;
    }

    public async Task<User> UpdateAsync(int id, User user, CancellationToken cancellationToken)
    {
        return user;
    }

    public async Task<User> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        return new()
        {
            Id = 0,
            Name = "aaa",
            Email = "",
            Role = Role.Regular,
            Groups = null
        };
    }

    public async Task<IEnumerable<User>> ListAllAsync(
        int take, 
        int skip, 
        CancellationToken cancellationToken) =>
        new List<User>
        {
            new()
            {
                Id = 0,
                Name = "aaa",
                Email = "",
                Role = Role.Regular,
                Groups = null
            }
        };
}