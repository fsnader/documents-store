using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Documents.Abstractions;

namespace DocumentsStore.UseCases.Documents;

public class GetDocumentById : IGetDocumentById
{
    private readonly IDocumentsRepository _documentsRepository;

    public GetDocumentById(IDocumentsRepository documentsRepository)
    {
        _documentsRepository = documentsRepository;
    }
    
    public async Task<UseCaseResult<Document>> ExecuteAsync(User user, int id, CancellationToken cancellationToken)
    {
        var document = await _documentsRepository.GetDocumentById(id, cancellationToken);

        if (document is null)
        {
            return UseCaseResult<Document>.NotFound();
        }
        
        var hasPermission = await _documentsRepository.CheckUserDocumentPermission(id, user.Id, cancellationToken);

        if (!hasPermission)
        {
            return UseCaseResult<Document>.Unauthorized();
        }
        
        return UseCaseResult<Document>.Success(document);
    }
}