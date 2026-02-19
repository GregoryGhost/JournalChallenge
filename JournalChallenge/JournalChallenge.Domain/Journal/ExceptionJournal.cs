namespace JournalChallenge.Domain.Journal;

public class ExceptionJournal
{
    public long Id { get; set; }
    
    public long EventId { get; set; }

    public DateTime Timestamp { get; set; }

    public string QueryParams { get; set; } = null!;
    public string BodyParams { get; set; } = null!;

    public string StackTrace { get; set; } = null!;

    public string ExceptionType { get; set; } = null!;
    public string Message { get; set; } = null!;
}