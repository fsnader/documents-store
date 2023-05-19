using Dapper;
using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.Repositories.Database;
using DocumentsStore.Repositories.Queries;

namespace DocumentsStore.Repositories;

public class GroupsRepository : IGroupsRepository
{
    private readonly IQueryExecutor _queryExecutor;

    public GroupsRepository(IQueryExecutor queryExecutor) => _queryExecutor = queryExecutor;

    public async Task<Group?> CreateAsync(Group group, CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(
            async connection =>
                await connection.QueryFirstOrDefaultAsync<Group>(GroupsQueries.Create, group),
            cancellationToken);

    public async Task<Group?> DeleteAsync(int id, CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(async connection =>
        {
            var parameters = new { Id = id };
            var result = await connection.QueryFirstOrDefaultAsync<Group>(GroupsQueries.Delete, parameters);
            return result;
        }, cancellationToken);

    public async Task<IEnumerable<Group>> ListAllAsync(int take, int skip, CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(async connection =>
        {
            var parameters = new
            {
                Take = take,
                Skip = skip
            };

            var result = await connection.QueryAsync<Group>(GroupsQueries.ListAll, parameters);
            return result;
        }, cancellationToken);

    public async Task<Group?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(async connection =>
        {
            var result = await connection.QueryFirstOrDefaultAsync<Group>(
                GroupsQueries.GetById,
                new { Id = id });

            return result;
        }, cancellationToken);

    public async Task<Group?> UpdateAsync(int id, Group group, CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(async connection =>
        {
            var parameters = new { Id = id, Name = group.Name };
            var result = await connection.QueryFirstOrDefaultAsync<Group>(GroupsQueries.Update, parameters);

            return result;
        }, cancellationToken);
}