using DocumentsStore.Api.DTOs;
using DocumentsStore.Domain;
using DocumentsStore.UseCases.Documents.Abstractions;
using DocumentsStore.UseCases.Users.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsStore.Api.Controllers;

[Route("api/documents")]
[ApiController]
public class DocumentsController : BaseController
{
    private readonly ICreateDocument _createDocument;
    private readonly IGetDocumentById _getDocumentById;
    private readonly IGetUserAuthorizedDocuments _getUserAuthorizedDocuments;
    private readonly IGetUserById _getUserBy;

    private async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var result = await _getUserBy.ExecuteAsync(1, cancellationToken);
        return result.Result!;
    }

    public DocumentsController(
        ICreateDocument createDocument,
        IGetDocumentById getDocumentById,
        IGetUserAuthorizedDocuments getUserAuthorizedDocuments, IGetUserById getUserBy)
    {
        _createDocument = createDocument;
        _getDocumentById = getDocumentById;
        _getUserAuthorizedDocuments = getUserAuthorizedDocuments;
        _getUserBy = getUserBy;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateDocumentDto document, CancellationToken cancellationToken)
    {
        var result = await _createDocument.ExecuteAsync(
            await GetCurrentUserAsync(cancellationToken),
            document.ConvertToDocument(),
            document.AuthorizedUsers,
            document.AuthorizedGroups,
            cancellationToken);

        return UseCaseActionResult(result, DocumentDto.CreateFromDocument);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var result = await _getDocumentById.ExecuteAsync(
            await GetCurrentUserAsync(cancellationToken),
            id, cancellationToken);

        return UseCaseActionResult(result, DocumentDto.CreateFromDocument);
    }

    [HttpGet]
    public async Task<IActionResult> GetList(
        CancellationToken cancellationToken,
        [FromQuery] int take = 100,
        [FromQuery] int skip = 0)
    {
        var result = await _getUserAuthorizedDocuments.ExecuteAsync(
            await GetCurrentUserAsync(cancellationToken),
            take,
            skip,
            cancellationToken);

        return UseCaseActionResult(result, DocumentDto.CreateFromDocuments);
    }
}