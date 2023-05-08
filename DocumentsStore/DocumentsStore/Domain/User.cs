namespace DocumentsStore.Domain;

public class User
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
    public Group Group { get; set; }
}