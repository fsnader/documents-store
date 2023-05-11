using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Documents.Abstractions;

namespace DocumentsStore.UseCases.Documents;

public class CreateDocument : ICreateDocument
{
    private readonly IDocumentsRepository _documentsRepository;

    public CreateDocument(IDocumentsRepository documentsRepository)
    {
        _documentsRepository = documentsRepository;
    }
    
    public async Task<UseCaseResult<Document>> ExecuteAsync(User user, Document document, int[] users, int[] groups,
        CancellationToken cancellationToken)
    {
        if (user.Role != Role.Admin && user.Role == Role.Manager)
        {
            return UseCaseResult<Document>.Unauthorized("User doesn't have permission to perform this action");
        }

        if (!IsValid(document))
        {
            return UseCaseResult<Document>.BadRequest("Please provide all document required fields");
        }
        

        if (!users.Any() && !groups.Any())
        {
            return UseCaseResult<Document>.BadRequest("Please provide a list of user or group permissions");
        }

        document.UserId = user.Id;

        var result = await _documentsRepository.CreateDocumentAsync(document, users, groups, cancellationToken);
        
        return UseCaseResult<Document>.Success(result);
    }

    private bool IsValid(Document document)
    {
        return true;
    }
}