using Dapper;
using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.Repositories.Database;
using DocumentsStore.Repositories.Queries;

namespace DocumentsStore.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UsersRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        using var db = _connectionFactory.GenerateConnection();
        var user = await db.QueryFirstOrDefaultAsync<User>(
            UserQueries.GetById,
            new { Id = id });
        
        return user;
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken)
    {
        using var db = _connectionFactory.GenerateConnection();

        var parameters = new
        {
            user.Name,
            user.Email,
            Role = user.Role.ToString()
        };

        var result = await db.ExecuteScalarAsync<User>(UserQueries.Create, parameters);

        return result;
    }

    public async Task<User?> UpdateAsync(int id, User user, CancellationToken cancellationToken)
    {
        using var db = _connectionFactory.GenerateConnection();

        var parameters = new
        {
            user.Name,
            user.Email,
            Role = user.Role.ToString(),
            Id = id
        };

        return await db.QueryFirstOrDefaultAsync<User>(UserQueries.Update, parameters);
    }

    public async Task<User?> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        using var db = _connectionFactory.GenerateConnection();

        var parameters = new { Id = id };

        return await db.QueryFirstOrDefaultAsync<User>(UserQueries.Delete, parameters);
    }

    public async Task<IEnumerable<User>> ListAllAsync(int take, int skip, CancellationToken cancellationToken)
    {
        using var db = _connectionFactory.GenerateConnection();
        
        var parameters = new
        {
            Take = take,
            Skip = skip
        };
        
        return await db.QueryAsync<User>(UserQueries.ListAll, parameters);
    }
}