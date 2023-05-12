using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Documents.Abstractions;

namespace DocumentsStore.UseCases.Documents;

public class GetUserAuthorizedDocuments : IGetUserAuthorizedDocuments
{
    private readonly IDocumentsRepository _documentsRepository;

    public GetUserAuthorizedDocuments(IDocumentsRepository documentsRepository)
    {
        _documentsRepository = documentsRepository;
    }

    public async Task<UseCaseResult<IEnumerable<Document>>> ExecuteAsync(User user, int take, int skip, CancellationToken cancellationToken)
    {
        var results = await _documentsRepository.ListUserAuthorizedDocumentsAsync(user.Id, take, skip, cancellationToken);
        
        return UseCaseResult<IEnumerable<Document>>.Success(results);
    }
}