using Dapper;
using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.Repositories.Database;
using DocumentsStore.Repositories.Queries;

namespace DocumentsStore.Repositories;

public class GroupUsersRepository : IGroupUsersRepository
{
    private readonly IQueryExecutor _queryExecutor;

    public GroupUsersRepository(IQueryExecutor queryExecutor) => _queryExecutor = queryExecutor;

    public async Task<IEnumerable<Group>> AddUserToGroupAsync(int userId, int groupId,
        CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(async connection =>
        {
            await connection.ExecuteAsync(
                GroupUsersQueries.AddUserToGroup,
                new
                {
                    UserId = userId,
                    GroupId = groupId
                });

            return await GetGroupsByUserIdAsync(userId, cancellationToken);
        }, cancellationToken);

    public async Task<IEnumerable<Group>> RemoveUserFromGroupAsync(int userId, int groupId,
        CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(async connection =>
        {
            await connection.ExecuteAsync(
                GroupUsersQueries.RemoveUserFromGroup,
                new
                {
                    UserId = userId,
                    GroupId = groupId
                });

            return await GetGroupsByUserIdAsync(userId, cancellationToken);
        }, cancellationToken);

    public async Task<IEnumerable<Group>> GetGroupsByUserIdAsync(int userId, CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(
            async connection => await connection.QueryAsync<Group>(
                GroupUsersQueries.GetGroupsByUserId,
                new { UserId = userId }), cancellationToken);
}