using System.Data;
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
        
        var parameters = new DynamicParameters();
        parameters.Add("user_id", document.UserId);
        parameters.Add("name", document.Name);
        parameters.Add("description", document.Description);
        parameters.Add("category", document.CategoryString);
        parameters.Add("content", document.Content);
        parameters.Add("posted_date", document.PostedDate);
        parameters.Add("authorized_users", authorizedUsers);
        parameters.Add("authorized_groups", authorizedGroups);
        parameters.Add("document_id", direction: ParameterDirection.InputOutput);

        await connection.ExecuteAsync("\"CreateDocument\"", parameters, commandType: CommandType.StoredProcedure);
        
        document.Id = parameters.Get<int>("document_id");

        return document;
    }

    public async Task<Document?> GetDocumentById(int id, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.GenerateConnection();

        var query = DocumentQueries.GetDocumentById;
        return await connection.QueryFirstOrDefaultAsync<Document>(query, new { Id = id });
    }

    public async Task<bool> CheckUserDocumentPermission(int id, int userId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.GenerateConnection();

        var query = DocumentQueries.CheckUserDocumentPermission;
        var result = await connection.QueryFirstOrDefaultAsync<bool>(query, new { DocumentId = id, UserId = userId });

        return result;
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