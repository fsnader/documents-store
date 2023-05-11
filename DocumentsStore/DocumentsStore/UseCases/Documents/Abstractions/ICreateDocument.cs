using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Documents.Abstractions;

public interface ICreateDocument
{
    Task<UseCaseResult<Document>> ExecuteAsync(
        User user, // Should we receive just userId?
        Document document, 
        int[] users,
        int[] groups,
        CancellationToken cancellationToken);
}