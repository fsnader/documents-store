using DocumentsStore.Domain;

namespace DocumentsStore.Api.DTOs.Documents;

public class DocumentWithPermissionsDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
    public string Content { get; set; }
    public DateTimeOffset PostedDate { get; set; }
    public IEnumerable<int> AuthorizedUsers { get; set; }
    public IEnumerable<int>  AuthorizedGroups { get; set; }

    public static DocumentWithPermissionsDto CreateFromDocument(Document document) =>
        new()
        {
            Id = document.Id,
            UserId = document.UserId,
            Name = document.Name,
            Description = document.Description,
            Category = document.Category,
            Content = document.Content,
            PostedDate = document.PostedDate,
            AuthorizedUsers = document.AuthorizedUsers,
            AuthorizedGroups = document.AuthorizedGroups,
        };
}