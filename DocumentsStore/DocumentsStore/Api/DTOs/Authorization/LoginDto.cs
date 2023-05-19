using System.ComponentModel.DataAnnotations;

namespace DocumentsStore.Api.DTOs.Authorization;

public record LoginDto
{
    [Required]
    public int Id { get; set; }
}