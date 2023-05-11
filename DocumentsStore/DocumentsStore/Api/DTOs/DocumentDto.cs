using DocumentsStore.Domain;

namespace DocumentsStore.Api.DTOs;

public class DocumentDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
    public string Content { get; set; }
    public DateTimeOffset PostedDate { get; set; }

    public static DocumentDto CreateFromDocument(Document document) =>
        new DocumentDto
        {
            Id = document.Id,
            UserId = document.UserId,
            Name = document.Name,
            Description = document.Description,
            Category = document.Category,
            Content = document.Content,
            PostedDate = document.PostedDate
        };

    public static IEnumerable<DocumentDto> CreateFromDocuments(IEnumerable<Document> documents)
        => documents.Select(CreateFromDocument);
}