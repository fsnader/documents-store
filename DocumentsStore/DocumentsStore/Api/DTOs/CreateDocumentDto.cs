using DocumentsStore.Domain;

namespace DocumentsStore.Api.DTOs;

public class CreateDocumentDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
    public string Content { get; set; }
    
    public IEnumerable<int> AuthorizedUsers { get; set; }
    public IEnumerable<int> AuthorizedGroups { get; set; }

    public Document ConvertToDocument() =>
        new Document
        {
            Name = Name,
            Description = Description,
            Category = Category,
            Content = Content,
            PostedDate = DateTimeOffset.UtcNow,
        };
}