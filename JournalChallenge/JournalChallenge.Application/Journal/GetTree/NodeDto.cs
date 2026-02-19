namespace JournalChallenge.Application.Journal.GetTree;

public sealed class NodeDto
{
    public long Id { get; init; }

    public string Name { get; init; } = null!;

    public IEnumerable<NodeDto> Children { get; init; } = new List<NodeDto>();
}
