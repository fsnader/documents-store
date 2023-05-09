using System.Data;
using Dapper;
using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.Repositories.Queries;
using Npgsql;

namespace DocumentsStore.Repositories;

public class GroupUsersRepository : IGroupUsersRepository
{
    private readonly string _connectionString;

    public GroupUsersRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Postgresql");
    }

    public async Task<User> AddUserToGroup(int userId, int groupId, CancellationToken cancellationToken)
    {
        using IDbConnection db = new NpgsqlConnection(_connectionString);
        var parameters = new { UserId = userId, GroupId = groupId };

        await db.ExecuteAsync(GroupUsersQueries.AddUserToGroup, parameters);

        return await db.QuerySingleAsync<User>(GroupUsersQueries.GetUserById, new { Id = userId });
    }

    public async Task<User> RemoveUserFromGroup(int userId, int groupId, CancellationToken cancellationToken)
    {
        using IDbConnection db = new NpgsqlConnection(_connectionString);
        
        var parameters = new { UserId = userId, GroupId = groupId };

        await db.ExecuteAsync(GroupUsersQueries.RemoveUserFromGroup, parameters);

        return await db.QuerySingleAsync<User>(GroupUsersQueries.GetUserById, new { Id = userId });
    }
}