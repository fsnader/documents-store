using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Documents.Abstractions;

public interface ICreateDocument
{
    Task<UseCaseResult<Document>> ExecuteAsync(
        User user, // Should we receive just userId?
        Document document, 
        IEnumerable<int> users,
        IEnumerable<int> groups,
        CancellationToken cancellationToken);
}