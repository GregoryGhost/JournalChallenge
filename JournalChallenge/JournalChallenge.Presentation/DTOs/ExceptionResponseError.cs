namespace JournalChallenge.Presentation.DTOs;

using Microsoft.AspNetCore.Mvc;

public class ExceptionResponseError: ProblemDetails, IResponseError
{
    public ExceptionResponseError(long eventId)
    {
        Type = "Exception";
        Data = new ResponseErrorMessage {Message = $"Internal server error ID = {eventId}"};
        Id = eventId.ToString();
    }
    
    public string Id { get; init; }

    public ResponseErrorMessage Data { get; init; }
}