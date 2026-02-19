namespace JournalChallenge.Domain.Journal;

public class Tree
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    
    public virtual ICollection<Node> Nodes { get; set; } = new List<Node>();
}
