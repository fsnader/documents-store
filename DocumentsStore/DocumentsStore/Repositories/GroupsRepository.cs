using Dapper;
using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.Repositories.Database;
using DocumentsStore.Repositories.Queries;

namespace DocumentsStore.Repositories;

public class GroupsRepository : IGroupsRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GroupsRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Group?> CreateAsync(Group group, CancellationToken cancellationToken)
    {
        const string query = GroupsQueries.Create;
        using var connection = _dbConnectionFactory.GenerateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<Group>(query, group);
        return result;
    }

    public async Task<Group?> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        const string query = GroupsQueries.Delete;
        using var connection = _dbConnectionFactory.GenerateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<Group>(query, new { Id = id });
        return result;
    }

    public async Task<IEnumerable<Group>> ListAllAsync(int take, int skip, CancellationToken cancellationToken)
    {
        const string query = GroupsQueries.ListAll;
        using var connection = _dbConnectionFactory.GenerateConnection();
        var result = await connection.QueryAsync<Group>(query, new { Take = take, Skip = skip });
        return result;
    }

    public async Task<Group?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        const string query = GroupsQueries.GetById;
        using var connection = _dbConnectionFactory.GenerateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<Group>(query, new { Id = id });
        return result;
    }

    public async Task<Group?> UpdateAsync(int id, Group group, CancellationToken cancellationToken)
    {
        const string query = GroupsQueries.Update;
        using var connection = _dbConnectionFactory.GenerateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<Group>(query, new { Id = id, Name = group.Name });
        return result;
    }
}