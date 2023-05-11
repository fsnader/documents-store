using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Documents.Abstractions;

public interface IGetDocumentById
{
    public Task<UseCaseResult<Document>> ExecuteAsync(User user, int id, CancellationToken cancellationToken);
}