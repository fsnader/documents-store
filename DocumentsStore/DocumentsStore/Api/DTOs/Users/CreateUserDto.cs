using DocumentsStore.Domain;

namespace DocumentsStore.Api.DTOs.Users;

public record CreateUserDto
{
    public string Name { get; set; }
    public string Email { get; set; }
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