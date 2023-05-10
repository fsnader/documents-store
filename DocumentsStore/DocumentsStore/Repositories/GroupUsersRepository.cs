using Dapper;
using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.Repositories.Database;
using DocumentsStore.Repositories.Queries;

namespace DocumentsStore.Repositories;

public class GroupUsersRepository : IGroupUsersRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GroupUsersRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Group>> AddUserToGroup(int userId, int groupId, CancellationToken cancellationToken)
    {
        using var db = _connectionFactory.GenerateConnection();

        var parameters = new { UserId = userId, GroupId = groupId };

        await db.ExecuteAsync(GroupUsersQueries.AddUserToGroup, parameters);

        return await GetGroupsByUserIdAsync(userId, cancellationToken);
    }

    public async Task<IEnumerable<Group>> RemoveUserFromGroup(int userId, int groupId, CancellationToken cancellationToken)
    {
        using var db = _connectionFactory.GenerateConnection();
        
        var parameters = new { UserId = userId, GroupId = groupId };

        await db.ExecuteAsync(GroupUsersQueries.RemoveUserFromGroup, parameters);

        return await GetGroupsByUserIdAsync(userId, cancellationToken);
    }
    
    public async Task<IEnumerable<Group>> GetGroupsByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        using var db = _connectionFactory.GenerateConnection();

        var parameters = new { UserId = userId };

        return await db.QueryAsync<Group>(GroupUsersQueries.GetGroupsByUserId, parameters);
    }
}