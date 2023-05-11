using DocumentsStore.Domain;
using DocumentsStore.UseCases.Documents.Abstractions;

namespace DocumentsStore.UseCases.Documents;

public class GetUserAuthorizedDocuments : IGetUserAuthorizedDocuments
{
    public Task<UseCaseResult<IEnumerable<Document>>> ExecuteAsync(User user, int take, int skip, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}