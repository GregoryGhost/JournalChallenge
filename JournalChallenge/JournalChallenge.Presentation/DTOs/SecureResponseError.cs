namespace JournalChallenge.Presentation.DTOs;

using Microsoft.AspNetCore.Mvc;

public class SecureResponseError: ProblemDetails, IResponseError
{
    public SecureResponseError()
    {
        Type = "Secure";
    }
    
    public required string Id { get; init; }

    public required ResponseErrorMessage Data { get; init; }
}