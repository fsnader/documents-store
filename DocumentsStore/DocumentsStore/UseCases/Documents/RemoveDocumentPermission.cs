using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Documents.Abstractions;

namespace DocumentsStore.UseCases.Documents;

public class RemoveDocumentPermission : IRemoveDocumentPermission
{
    private readonly IDocumentsRepository _documentsRepository;

    public RemoveDocumentPermission(IDocumentsRepository documentsRepository)
    {
        _documentsRepository = documentsRepository;
    }

    public async Task<UseCaseResult<Document>> ExecuteAsync(User user, int documentId, int permissionId,
        PermissionType type,
        CancellationToken cancellationToken)
    {
        var documentTask = _documentsRepository.GetDocumentByIdAsync(documentId, cancellationToken);

        var document = await documentTask;

        if (document is null)
        {
            return UseCaseResult<Document>.NotFound();
        }

        if (document.UserId != user.Id)
        {
            return UseCaseResult<Document>.Unauthorized();
        }
        
        await RemovePermissionAsync(documentId, permissionId, type, cancellationToken);

        var authorizedUsers = _documentsRepository.GetDocumentUsersPermissionsAsync(documentId, cancellationToken);
        var authorizedGroups = _documentsRepository.GetDocumentGroupsPermissionsAsync(documentId, cancellationToken);

        document.AuthorizedUsers = await authorizedUsers;
        document.AuthorizedGroups = await authorizedGroups;

        return UseCaseResult<Document>.Success(document);
    }

    private Task RemovePermissionAsync(int documentId, int permissionId, PermissionType type,
        CancellationToken cancellationToken) =>
        type switch
        {
            PermissionType.Group => _documentsRepository.RemoveDocumentGroupsPermissionAsync(
                documentId,
                permissionId,
                cancellationToken),

            PermissionType.User => _documentsRepository.RemoveDocumentUserPermissionAsync(
                documentId,
                permissionId,
                cancellationToken),

            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
}