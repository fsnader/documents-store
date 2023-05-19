using System.Data;
using Dapper;
using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.Repositories.Database;
using DocumentsStore.Repositories.Exceptions;
using DocumentsStore.Repositories.Queries;
using Npgsql;

namespace DocumentsStore.Repositories;

public class DocumentsRepository : IDocumentsRepository
{
    private readonly IQueryExecutor _queryExecutor;
    public DocumentsRepository(IQueryExecutor queryExecutor) => _queryExecutor = queryExecutor;

    public async Task<Document> CreateDocumentAsync(
        Document document,
        IEnumerable<int> authorizedUsers,
        IEnumerable<int> authorizedGroups,
        CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(
            async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("user_id", document.UserId);
                parameters.Add("name", document.Name);
                parameters.Add("description", document.Description);
                parameters.Add("category", document.Category.ToString());
                parameters.Add("content", document.Content);
                parameters.Add("posted_date", document.PostedDate);
                parameters.Add("authorized_users", authorizedUsers);
                parameters.Add("authorized_groups", authorizedGroups);
                parameters.Add("document_id", direction: ParameterDirection.InputOutput);

                await connection.ExecuteAsync("create_document", parameters, commandType: CommandType.StoredProcedure);

                document.Id = parameters.Get<int>("document_id");

                return document;
            }, cancellationToken);

    public async Task<Document?> GetDocumentByIdAsync(int id, CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(
            async connection =>
                await connection.QueryFirstOrDefaultAsync<Document>(DocumentQueries.GetDocumentById, new { Id = id }),
            cancellationToken);

    public async Task AddDocumentUserPermissionAsync(
        int documentId,
        int userId,
        CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(
            async connection =>
                await connection.ExecuteAsync(DocumentQueries.InsertUsePermission,
                    new
                    {
                        DocumentId = documentId,
                        UserId = userId,
                    }), cancellationToken);

    public async Task RemoveDocumentUserPermissionAsync(int documentId, int userId, CancellationToken cancellationToken)
        => await _queryExecutor.ExecuteQueryAsync(
            async connection =>
                await connection.ExecuteAsync(DocumentQueries.RemoveUserPermission, new
                {
                    DocumentId = documentId,
                    UserId = userId,
                }), cancellationToken);

    public async Task AddDocumentGroupsPermissionAsync(
        int documentId,
        int groupId,
        CancellationToken cancellationToken)
        => await _queryExecutor.ExecuteQueryAsync(
            async connection =>
                await connection.ExecuteAsync(DocumentQueries.InsertGroupPermission, new
                {
                    DocumentId = documentId,
                    GroupId = groupId,
                }), cancellationToken);

    public async Task RemoveDocumentGroupsPermissionAsync(int documentId, int groupId,
        CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(async connection =>
            await connection.ExecuteAsync(DocumentQueries.RemoveGroupPermission, new
            {
                DocumentId = documentId,
                GroupId = groupId,
            }), cancellationToken);

    public async Task<IEnumerable<int>> GetDocumentUsersPermissionsAsync(int documentId,
        CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(async connection =>
                await connection.QueryAsync<int>(DocumentQueries.GetUsersPermissions,
                    new { DocumentId = documentId }),
            cancellationToken);

    public async Task<IEnumerable<int>> GetDocumentGroupsPermissionsAsync(int documentId,
        CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(
            async connection =>
                await connection.QueryAsync<int>(
                    DocumentQueries.GetGroupsPermissions, 
                    new { DocumentId = documentId }),
            cancellationToken);

    public async Task<bool> CheckUserDocumentPermissionAsync(int id, int userId,
        CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(
            async connection =>
                await connection.QueryFirstOrDefaultAsync<bool>(
                    DocumentQueries.CheckUserDocumentPermission,
                    new { DocumentId = id, UserId = userId }), 
            cancellationToken);

    public async Task<IEnumerable<Document>> ListUserAuthorizedDocumentsAsync(int userId, int take, int skip,
        CancellationToken cancellationToken) =>
        await _queryExecutor.ExecuteQueryAsync(async connection => await connection.QueryAsync<Document>(
            DocumentQueries.ListUserAuthorizedDocuments,
            new
            {
                UserId = userId,
                Take = take,
                Skip = skip
            }), cancellationToken);
}