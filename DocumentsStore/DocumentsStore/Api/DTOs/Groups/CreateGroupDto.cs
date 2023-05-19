using System.ComponentModel.DataAnnotations;
using DocumentsStore.Domain;

namespace DocumentsStore.Api.DTOs.Groups;

public class CreateGroupDto
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Name { get; set; }
    
    public Group ConvertToGroup() =>
        new()
        {
            Id = Id,
            Name = Name,
        };
}