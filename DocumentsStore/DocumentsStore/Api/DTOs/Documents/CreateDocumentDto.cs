using System.ComponentModel.DataAnnotations;
using DocumentsStore.Domain;

namespace DocumentsStore.Api.DTOs.Documents;

public class CreateDocumentDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    [Required]
    [StringLength(300)]
    public string Description { get; set; }
    
    [Required]
    public Category Category { get; set; }
    
    [Required]
    [StringLength(3000)]
    public string Content { get; set; }
    
    [Required]
    public int[] AuthorizedUsers { get; set; }
    
    [Required]
    public int[]  AuthorizedGroups { get; set; }

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