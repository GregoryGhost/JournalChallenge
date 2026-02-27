namespace JournalChallenge.Presentation.DTOs;

public record ResponseErrorMessage
{
    public string Message { get; init; } = null!;

    public IDictionary<string,string[]>? Errors { get; init; }
}