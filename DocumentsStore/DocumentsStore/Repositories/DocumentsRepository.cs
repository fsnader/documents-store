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
    }

    public async Task<Document?> GetDocumentByIdAsync(int id, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.GenerateConnection();

        var query = DocumentQueries.GetDocumentById;
        return await connection.QueryFirstOrDefaultAsync<Document>(query, new { Id = id });
    }

    public async Task AddDocumentUserPermissionAsync(
        int documentId, 
        int userId,
        CancellationToken cancellationToken)
    {
        try
        {
            using var connection = _dbConnectionFactory.GenerateConnection();
            
            var query = DocumentQueries.InsertUsePermission;
            await connection.ExecuteAsync(query, new
            {
                DocumentId = documentId,
                UserId = userId,
            });
        }
        catch (PostgresException ex)
        {
            if (ex.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                throw new UniqueException();
            }
            
            throw;
        }
    }

    public async Task RemoveDocumentUserPermissionAsync(int documentId, int userId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.GenerateConnection();
            
        var query = DocumentQueries.RemoveUserPermission;
        
        await connection.ExecuteAsync(query, new
        {
            DocumentId = documentId,
            UserId = userId,
        });
    }

    public async Task AddDocumentGroupsPermissionAsync(
        int documentId, 
        int groupId,
        CancellationToken cancellationToken)
    {
        try
        {
            using var connection = _dbConnectionFactory.GenerateConnection();
            
            var query = DocumentQueries.InsertGroupPermission;
            await connection.ExecuteAsync(query, new
            {
                DocumentId = documentId,
                GroupId = groupId,
            });
        }
        catch (PostgresException ex)
        {
            if (ex.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                throw new UniqueException();
            }
            
            throw;
        }
    }

    public async Task RemoveDocumentGroupsPermissionAsync(int documentId, int groupId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.GenerateConnection();
            
        var query = DocumentQueries.RemoveGroupPermission;
        
        await connection.ExecuteAsync(query, new
        {
            DocumentId = documentId,
            GroupId = groupId,
        });
    }

    public async Task<IEnumerable<int>> GetDocumentUsersPermissionsAsync(int documentId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.GenerateConnection();

        var query = DocumentQueries.GetUsersPermissions;
        return await connection.QueryAsync<int>(query,new { DocumentId = documentId });
    }
    
    public async Task<IEnumerable<int>> GetDocumentGroupsPermissionsAsync(int documentId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.GenerateConnection();

        var query = DocumentQueries.GetGroupsPermissions;
        return await connection.QueryAsync<int>(query,new { DocumentId = documentId });
    }

    public async Task<bool> CheckUserDocumentPermissionAsync(int id, int userId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.GenerateConnection();

        var query = DocumentQueries.CheckUserDocumentPermission;
        return await connection.QueryFirstOrDefaultAsync<bool>(query, new { DocumentId = id, UserId = userId });
    }

    public async Task<IEnumerable<Document>> ListUserAuthorizedDocumentsAsync(int userId, int take, int skip, CancellationToken cancellationToken)
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