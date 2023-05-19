using Dapper;
using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.Repositories.Database;
using DocumentsStore.Repositories.Queries;

namespace DocumentsStore.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly IQueryExecutor _queryExecutor;
    public UsersRepository(IQueryExecutor queryExecutor) => _queryExecutor = queryExecutor;

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(async connection =>
                await connection.QueryFirstOrDefaultAsync<User>(
                    UserQueries.GetById,
                    new { Id = id }),
            cancellationToken);

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(async connection =>
                await connection.QueryFirstOrDefaultAsync<User>(
                    UserQueries.Create,
                    new
                    {
                        user.Name,
                        user.Email,
                        Role = user.Role.ToString()
                    }),
            cancellationToken);

    public async Task<User?> UpdateAsync(int id, User user, CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(async connection =>
                await connection.QueryFirstOrDefaultAsync<User>(
                    UserQueries.Update, new
                    {
                        user.Name,
                        user.Email,
                        Role = user.Role.ToString(),
                        Id = id
                    }),
            cancellationToken);

    public async Task<User?> DeleteAsync(int id, CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(async connection => await connection.QueryFirstOrDefaultAsync<User>(
                UserQueries.Delete,
                new { Id = id }),
            cancellationToken);

    public async Task<IEnumerable<User>> ListAllAsync(int take, int skip, CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(async connection => await connection.QueryAsync<User>(
                UserQueries.ListAll, new
                {
                    Take = take,
                    Skip = skip
                }),
            cancellationToken);
}