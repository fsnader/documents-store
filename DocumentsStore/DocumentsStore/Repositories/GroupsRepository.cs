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
        using var connection = _dbConnectionFactory.GenerateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<Group>(GroupsQueries.Create, group);
        return result;
    }

    public async Task<Group?> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.GenerateConnection();
        var parameters = new { Id = id };
        var result = await connection.QueryFirstOrDefaultAsync<Group>(GroupsQueries.Delete, parameters);
        return result;
    }

    public async Task<IEnumerable<Group>> ListAllAsync(int take, int skip, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.GenerateConnection();

        var parameters = new
        {
            Take = take,
            Skip = skip
        };

        var result = await connection.QueryAsync<Group>(GroupsQueries.ListAll, parameters);
        return result;
    }

    public async Task<Group?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.GenerateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<Group>(
            GroupsQueries.GetById, 
            new { Id = id });
        
        return result;
    }

    public async Task<Group?> UpdateAsync(int id, Group group, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.GenerateConnection();
        var parameters = new { Id = id, Name = group.Name };
        var result = await connection.QueryFirstOrDefaultAsync<Group>(GroupsQueries.Update, parameters);
        
        return result;
    }
}