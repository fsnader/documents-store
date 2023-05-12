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
        var documentTask = _documentsRepository.GetDocumentById(id, cancellationToken);
        var hasPermission = _documentsRepository.CheckUserDocumentPermission(id, user.Id, cancellationToken);
        var authorizedUsers = _documentsRepository.GetDocumentUsersPermissionsAsync(id, cancellationToken);
        var authorizedGroups = _documentsRepository.GetDocumentGroupsPermissionsAsync(id, cancellationToken);
        
        var document = await documentTask;
        

        if (document is null)
        {
            return UseCaseResult<Document>.NotFound();
        }
        
        if (!await hasPermission)
        {
            return UseCaseResult<Document>.Unauthorized();
        }
        
        document.AuthorizedUsers = await authorizedUsers;
        document.AuthorizedGroups = await authorizedGroups;
        
        return UseCaseResult<Document>.Success(document);
    }
}