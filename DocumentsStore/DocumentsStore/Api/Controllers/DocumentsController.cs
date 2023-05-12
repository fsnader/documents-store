using DocumentsStore.Api.Authorization;
using DocumentsStore.Api.DTOs.Documents;
using DocumentsStore.Domain;
using DocumentsStore.UseCases.Documents.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsStore.Api.Controllers;

[Route("api/documents")]
[ApiController]
public class DocumentsController : BaseController
{
    private readonly ICreateDocument _createDocument;
    private readonly IGetDocumentById _getDocumentById;
    private readonly IGetUserAuthorizedDocuments _getUserAuthorizedDocuments;
    private readonly IUserService _userService;
    private readonly IAddDocumentPermission _addDocumentPermission;
    private readonly IRemoveDocumentPermission _removeDocumentPermission;

    public DocumentsController(
        ICreateDocument createDocument,
        IGetDocumentById getDocumentById,
        IGetUserAuthorizedDocuments getUserAuthorizedDocuments,
        IUserService userService, 
        IAddDocumentPermission addDocumentPermission, 
        IRemoveDocumentPermission removeDocumentPermission)
    {
        _createDocument = createDocument;
        _getDocumentById = getDocumentById;
        _getUserAuthorizedDocuments = getUserAuthorizedDocuments;
        _userService = userService;
        _addDocumentPermission = addDocumentPermission;
        _removeDocumentPermission = removeDocumentPermission;
    }

    [HttpPost, Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(DocumentDto), 200)]
    public async Task<IActionResult> Post([FromBody] CreateDocumentDto document, CancellationToken cancellationToken)
    {
        var result = await _createDocument.ExecuteAsync(
            await _userService.GetCurrentUserAsync(cancellationToken),
            document.ConvertToDocument(),
            document.AuthorizedUsers,
            document.AuthorizedGroups,
            cancellationToken);

        return UseCaseActionResult(result, DocumentDto.CreateFromDocument);
    }

    [HttpGet("{id}"), Authorize(Roles = "Admin,Manager,Regular")]
    [ProducesResponseType(typeof(DocumentWithPermissionsDto), 200)]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var result = await _getDocumentById.ExecuteAsync(
            await _userService.GetCurrentUserAsync(cancellationToken),
            id, cancellationToken);

        return UseCaseActionResult(result, DocumentWithPermissionsDto.CreateFromDocument);
    }

    [HttpGet, Authorize(Roles = "Admin,Manager,Regular")]
    [ProducesResponseType(typeof(DocumentDto[]), 200)]
    public async Task<IActionResult> GetList(
        CancellationToken cancellationToken,
        [FromQuery] int take = 100,
        [FromQuery] int skip = 0)
    {
        var user = await _userService.GetCurrentUserAsync(cancellationToken);
        
        var result = await _getUserAuthorizedDocuments.ExecuteAsync(
            user,
            take,
            skip,
            cancellationToken);

        return UseCaseActionResult(result, DocumentDto.CreateFromDocuments);
    }

    [HttpPost("{documentId}/users/{userId}"), Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(DocumentWithPermissionsDto), 200)]
    public async Task<IActionResult> AddUserPermissionAsync(
        int documentId, 
        int userId,
        CancellationToken cancellationToken)
    {
        var user = await  _userService.GetCurrentUserAsync(cancellationToken);
        var result = await _addDocumentPermission.ExecuteAsync(user, documentId, userId, PermissionType.User, cancellationToken);

        return UseCaseActionResult(result, DocumentWithPermissionsDto.CreateFromDocument);
    }
    
    [HttpPost("{documentId}/groups/{userId}"), Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(DocumentWithPermissionsDto), 200)]
    public async Task<IActionResult> AddGroupPermissionAsync(
        int documentId, 
        int userId,
        CancellationToken cancellationToken)
    {
        var user = await  _userService.GetCurrentUserAsync(cancellationToken);
        var result = await _addDocumentPermission.ExecuteAsync(user, documentId, userId, PermissionType.Group, cancellationToken);

        return UseCaseActionResult(result, DocumentWithPermissionsDto.CreateFromDocument);
    }
    
    [HttpDelete("{documentId}/users/{userId}"), Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(DocumentWithPermissionsDto), 200)]
    public async Task<IActionResult> DeleteUserPermissionAsync(
        int documentId, 
        int userId,
        CancellationToken cancellationToken)
    {
        var user = await  _userService.GetCurrentUserAsync(cancellationToken);
        var result = await _removeDocumentPermission.ExecuteAsync(user, documentId, userId, PermissionType.User, cancellationToken);

        return UseCaseActionResult(result, DocumentWithPermissionsDto.CreateFromDocument);
    }
    
    [HttpDelete("{documentId}/groups/{groupId}"), Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(DocumentWithPermissionsDto), 200)]
    public async Task<IActionResult> DeleteGroupPermissionAsync(
        int documentId, 
        int groupId,
        CancellationToken cancellationToken)
    {
        var user = await  _userService.GetCurrentUserAsync(cancellationToken);
        var result = await _removeDocumentPermission.ExecuteAsync(user, documentId, groupId, PermissionType.Group, cancellationToken);

        return UseCaseActionResult(result, DocumentWithPermissionsDto.CreateFromDocument);
    }
}