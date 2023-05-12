using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Documents.Abstractions;

public interface IRemoveDocumentPermission
{
    public Task<UseCaseResult<Document>> ExecuteAsync(User user, int documentId, int permissionId, PermissionType type, CancellationToken cancellationToken);
}