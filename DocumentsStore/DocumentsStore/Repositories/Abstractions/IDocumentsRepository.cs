
using DocumentsStore.Domain;

namespace DocumentsStore.Repositories.Abstractions;

public interface IDocumentsRepository
{
    public Task<Document> CreateDocumentAsync(
        Document document,
        IEnumerable<int> authorizedUsers,
        IEnumerable<int> authorizedGroups,
        CancellationToken cancellationToken);

    public Task<Document?> GetDocumentByIdAsync(int id, CancellationToken cancellationToken);

    public Task<IEnumerable<int>> GetDocumentUsersPermissionsAsync(
        int documentId,
        CancellationToken cancellationToken);

    public Task AddDocumentUserPermissionAsync(
        int documentId,
        int userId,
        CancellationToken cancellationToken);
    
    public Task RemoveDocumentUserPermissionAsync(
        int documentId,
        int userId,
        CancellationToken cancellationToken);
    
    public Task<IEnumerable<int>>  GetDocumentGroupsPermissionsAsync(
        int documentId,
        CancellationToken cancellationToken);
    
    public Task AddDocumentGroupsPermissionAsync(
        int documentId,
        int groupId,
        CancellationToken cancellationToken);
    
    public Task RemoveDocumentGroupsPermissionAsync(
        int documentId,
        int groupId,
        CancellationToken cancellationToken);

    public Task<bool> CheckUserDocumentPermissionAsync(int id, int userId, CancellationToken cancellationToken);

    public Task<IEnumerable<Document>> ListUserAuthorizedDocumentsAsync(
        int userId,
        int take, 
        int skip,
        CancellationToken cancellationToken);
}