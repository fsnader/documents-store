using DocumentsStore.Domain;
using DocumentsStore.UseCases.Documents.Abstractions;

namespace DocumentsStore.UseCases.Documents;

public class GetDocumentById : IGetDocumentById
{
    public Task<UseCaseResult<Document>> ExecuteAsync(User user, int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}