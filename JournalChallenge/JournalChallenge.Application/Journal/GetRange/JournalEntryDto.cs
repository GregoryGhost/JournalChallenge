namespace JournalChallenge.Application.Journal.GetRange;

public sealed class JournalEntryDto
{
    public long Id { get; init; }
    public string EventId { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
}

public sealed class JournalRangeResponse
{
    public int Skip { get; init; }
    public int Count { get; init; }
    public IEnumerable<JournalEntryDto> Items { get; init; } = new List<JournalEntryDto>();
}
