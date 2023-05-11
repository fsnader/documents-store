
using DocumentsStore.Domain;

namespace DocumentsStore.Repositories.Abstractions;

public interface IDocumentsRepository
{
    public Task<Document> CreateDocumentAsync(
        Document document,
        IEnumerable<int> authorizedUsers,
        IEnumerable<int> authorizedGroups,
        CancellationToken cancellationToken);

    public Task<Document> GetDocumentById(int id, CancellationToken cancellationToken);

    public Task<bool> CheckUserDocumentPermission(int id, int userId, CancellationToken cancellationToken);

    public Task<IEnumerable<Document>> ListUserAuthorizedDocuments(
        int userId,
        int take, 
        int skip,
        CancellationToken cancellationToken);
}