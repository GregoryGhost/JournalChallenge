namespace JournalChallenge.Application.Journal.GetSingle;

public sealed class JournalDetailDto
{
    public string Text { get; init; } = null!;
    public long Id { get; init; }
    public string EventId { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
}
