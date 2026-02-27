namespace JournalChallenge.Presentation.DTOs;

using Microsoft.AspNetCore.Mvc;

public class ValidationResponseError: ProblemDetails, IResponseError
{
    public ValidationResponseError()
    {
        Type = "Validation";
    }
    
    public required string Id { get; init; }

    public required ResponseErrorMessage Data { get; init; }
}