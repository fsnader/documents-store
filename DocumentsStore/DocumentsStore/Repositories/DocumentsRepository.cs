using Dapper;
using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.Repositories.Database;
using DocumentsStore.Repositories.Queries;

namespace DocumentsStore.Repositories;

public class DocumentsRepository : IDocumentsRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DocumentsRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Document> CreateDocumentAsync(
        Document document, 
        IEnumerable<int> authorizedUsers, 
        IEnumerable<int> authorizedGroups, 
        CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.GenerateConnection();
        connection.Open();

        // Begin transaction
        using var transaction = connection.BeginTransaction();

        try
        {
            // Insert the document
            var insertQuery = DocumentQueries.CreateDocument;
            var documentId = await connection.ExecuteScalarAsync<int>(insertQuery, document, transaction);

            // Insert the authorized users
            var insertAuthorizedUserQuery = DocumentQueries.InsertAuthorizedUser;
            foreach (var userId in authorizedUsers)
            {
                await connection.ExecuteAsync(insertAuthorizedUserQuery, new { DocumentId = documentId, UserId = userId }, transaction);
            }

            // Insert the authorized groups
            var insertAuthorizedGroupQuery = DocumentQueries.InsertAuthorizedGroup;
            foreach (var groupId in authorizedGroups)
            {
                await connection.ExecuteAsync(insertAuthorizedGroupQuery, new { DocumentId = documentId, GroupId = groupId }, transaction);
            }

            // Commit transaction
            transaction.Commit();

            return await GetDocumentById(documentId, cancellationToken);
        }
        catch (Exception)
        {
            // Rollback transaction on error
            transaction.Rollback();
            throw;
        }
    }

    public async Task<Document> GetDocumentById(int id, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.GenerateConnection();

        var query = DocumentQueries.GetDocumentById;
        return await connection.QueryFirstOrDefaultAsync<Document>(query, new { Id = id });
    }

    public async Task<bool> CheckUserDocumentPermission(int id, int userId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.GenerateConnection();

        var query = DocumentQueries.CheckUserDocumentPermission;
        var result = await connection.QueryFirstOrDefaultAsync<int>(query, new { Id = id, UserId = userId });

        return result > 0;
    }

    public async Task<IEnumerable<Document>> ListUserAuthorizedDocuments(int userId, int take, int skip, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.GenerateConnection();

        var query = DocumentQueries.ListUserAuthorizedDocuments;
        return await connection.QueryAsync<Document>(query, 
            new
            {
                UserId = userId,
                Take = take, 
                Skip = skip
            });
    }
}