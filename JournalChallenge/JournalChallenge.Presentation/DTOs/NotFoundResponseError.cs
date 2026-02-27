namespace JournalChallenge.Presentation.DTOs;

using Microsoft.AspNetCore.Mvc;

public class NotFoundResponseError: ProblemDetails, IResponseError
{
    public NotFoundResponseError()
    {
        Type = "NotFound";
    }
    
    public required string Id { get; init; }

    public required ResponseErrorMessage Data { get; init; }
}