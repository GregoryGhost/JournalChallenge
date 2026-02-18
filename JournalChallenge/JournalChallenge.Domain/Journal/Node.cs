namespace JournalChallenge.Domain.Journal;

public class Node
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? ParentId { get; set; }
    
    public virtual Node? Parent { get; set; }

    public virtual ICollection<Node> Children { get; set; } = new List<Node>();
    
    public int TreeId { get; set; }
}