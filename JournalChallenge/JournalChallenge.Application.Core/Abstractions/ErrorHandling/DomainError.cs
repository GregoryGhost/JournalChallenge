namespace JournalChallenge.Application.Core.Abstractions.ErrorHandling;

using CSharpFunctionalExtensions;

public class DomainError(string message, ErrorType errorType): IError<DomainError>
{
    public string Message { get; } = message;

    public ErrorType Type { get; } = errorType;
    public static IError<DomainError> Failure(string message) =>
        new DomainError(message, ErrorType.Failure);

    public static IError<DomainError> NotFound(string message) =>
        new DomainError(message, ErrorType.NotFound);

    public static IError<DomainError> Problem(string message) =>
        new DomainError(message, ErrorType.Problem);

    public static IError<DomainError> Conflict(string message) =>
        new DomainError(message, ErrorType.Conflict);    
    public static IError<DomainError> Unauthenticated(string message) =>
        new DomainError(message, ErrorType.Unauthenticated);
    
    public static IError<DomainError> Validation(string message) =>
        new DomainError(message, ErrorType.Validation);

    public DomainError Error => this;
}