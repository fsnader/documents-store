using DocumentsStore.Domain;

namespace DocumentsStore.Api.DTOs.Users;

public record UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }

    public static UserDto CreateFromUser(User user) =>
        new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
        };

    public static IEnumerable<UserDto> CreateFromUsers(IEnumerable<User> users)
        => users.Select(CreateFromUser);

    public User ConvertToUser()
    {
        return new User
        {
            Id = Id,
            Name = Name,
            Email = Email,
            Role = Role,
        };
    }
}

