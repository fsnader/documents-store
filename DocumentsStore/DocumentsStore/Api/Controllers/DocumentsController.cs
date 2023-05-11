using DocumentsStore.Api.DTOs;
using DocumentsStore.Domain;
using DocumentsStore.UseCases.Documents.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsStore.Api.Controllers;

[Route("api/documents")]
[ApiController]
public class DocumentsController : BaseController
{
    private readonly ICreateDocument _createDocument;
    private readonly IGetDocumentById _getDocumentById;
    private readonly IGetUserAuthorizedDocuments _getUserAuthorizedDocuments;
    
    public DocumentsController(
        ICreateDocument createDocument, 
        IGetDocumentById getDocumentById, 
        IGetUserAuthorizedDocuments getUserAuthorizedDocuments)
    {
        _createDocument = createDocument;
        _getDocumentById = getDocumentById;
        _getUserAuthorizedDocuments = getUserAuthorizedDocuments;
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateDocumentDto document, CancellationToken cancellationToken)
    {
        var result = await _createDocument.ExecuteAsync(
            new User(),
            document.ConvertToDocument(),
            document.AuthorizedUsers,
            document.AuthorizedGroups,
            cancellationToken);

        return UseCaseActionResult(result, DocumentDto.CreateFromDocument);
    }
    
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var result = await _getDocumentById.ExecuteAsync(new User(), id, cancellationToken);
        
        return UseCaseActionResult(result, DocumentDto.CreateFromDocument);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetList(
        CancellationToken cancellationToken,
        [FromQuery] int take = 100,
        [FromQuery] int skip = 0)
    {
        var result = await _getUserAuthorizedDocuments.ExecuteAsync(new User(), take, skip, cancellationToken);
        
        return UseCaseActionResult(result, DocumentDto.CreateFromDocuments);
    }
}