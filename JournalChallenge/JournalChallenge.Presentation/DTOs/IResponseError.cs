namespace JournalChallenge.Presentation.DTOs;

public interface IResponseError
{
    public string Id { get; init; }
    public ResponseErrorMessage Data { get; init; }
}