using DocumentsStore.Domain;

namespace DocumentsStore.UseCases.Documents.Abstractions;

public interface IGetUserAuthorizedDocuments
{
    public Task<UseCaseResult<IEnumerable<Document>>> ExecuteAsync(User user, int take, int skip, CancellationToken cancellationToken);
}