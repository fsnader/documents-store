namespace DocumentsStore.Domain;

public class Document
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
    public string Content { get; set; }
    public DateTimeOffset PostedDate { get; set; }
    public IEnumerable<int> AuthorizedUsers { get; set; }
    public IEnumerable<int>  AuthorizedGroups { get; set; }
}