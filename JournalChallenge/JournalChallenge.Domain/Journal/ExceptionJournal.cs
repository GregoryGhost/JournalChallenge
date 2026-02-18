namespace JournalChallenge.Domain.Journal;

public class ExceptionJournal
{
    public int Id { get; set; }
    
    public long EventId { get; set; }

    public DateTime Timestamp { get; set; }

    public string QueryParams { get; set; }
    public string BodyParams { get; set; }

    public string StackTrace { get; set; }

    public string ExceptionType { get; set; }
    public string Message { get; set; }
}