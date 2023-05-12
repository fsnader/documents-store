using DocumentsStore.Domain;
using DocumentsStore.UseCases.Documents.Abstractions;

namespace DocumentsStore.UseCases.Documents;

public class RemoveDocumentPermission : IRemoveDocumentPermission
{
    public Task<UseCaseResult<Document>> ExecuteAsync(User user, int documentId, int permissionId, PermissionType type,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}