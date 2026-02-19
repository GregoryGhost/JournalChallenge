namespace JournalChallenge.Application.Core.Abstractions.ErrorHandling;

public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    Problem = 2,
    NotFound = 3,
    Conflict = 4,
    Unauthenticated = 5,
    Secure = 6
}