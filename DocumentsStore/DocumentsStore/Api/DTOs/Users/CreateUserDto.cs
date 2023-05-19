using System.ComponentModel.DataAnnotations;
using DocumentsStore.Domain;

namespace DocumentsStore.Api.DTOs.Users;

public record CreateUserDto
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; }
    
    [Required]
    public Role Role { get; set; }

    public User ConvertToUser()
    {
        return new User
        {
            Name = Name,
            Email = Email,
            Role = Role,
        };
    }
}