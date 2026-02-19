namespace JournalChallenge.Domain.Journal;

public class Node
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long? ParentId { get; set; }
    
    public virtual Node? Parent { get; set; }

    public virtual ICollection<Node> Children { get; set; } = new List<Node>();
    
    public long TreeId { get; set; }
    
    public virtual Tree Tree { get; set; } = null!;
}