using DocumentsStore.Domain;

namespace DocumentsStore.Api.DTOs;

public class GroupDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public static GroupDto CreateFromGroup(Group group) =>
        new GroupDto
        {
            Id = group.Id,
            Name = group.Name
        };

    public static IEnumerable<GroupDto> CreateFromGroups(IEnumerable<Group> groups) 
        => groups.Select(CreateFromGroup);

    public Group ConvertToGroup() =>
        new Group
        {
            Id = Id,
            Name = Name,
        };
}