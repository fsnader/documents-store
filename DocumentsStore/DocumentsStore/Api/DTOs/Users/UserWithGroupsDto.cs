using DocumentsStore.Domain;

namespace DocumentsStore.Api.DTOs.Users;

public record UserWithGroupsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
    public IEnumerable<GroupDto>? Groups { get; set; }

    public static UserWithGroupsDto CreateFromUser(User user) =>
        new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Groups = user.Groups?.Select(GroupDto.CreateFromGroup)
        };
}

