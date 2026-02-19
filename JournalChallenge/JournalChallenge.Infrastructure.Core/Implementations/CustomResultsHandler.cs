namespace JournalChallenge.Infrastructure.Core.Implementations;

using System.Text;

using CSharpFunctionalExtensions;

using JournalChallenge.Domain.Journal;
using JournalChallenge.Application.Core.Abstractions.ErrorHandling;
using JournalChallenge.Infrastructure.Core.Abstractions;


public interface IRestCustomResultsHandler: ICustomResultsHandler<DomainError, Exception>
{
}

internal sealed class CustomResultsHandler : IRestCustomResultsHandler
{
    public Exception MatchProblem(IError<DomainError> error)
    {
        return error.Error.Type switch {
            ErrorType.Secure => new SecureException(error.Error.Message),
            _ => new Exception(error.Error.Message)
        };
    }

    public Exception MatchProblem(IEnumerable<IError<DomainError>> errors)
    {
        var matchedErrors = FoldErrors(errors);
        var matchedProblem = MatchProblem(matchedErrors);
        
        return matchedProblem;
    }
    
    public IError<DomainError> FoldErrors(IEnumerable<IError<DomainError>> errors)
    {
        var array = errors as IError<DomainError>[] ?? errors.ToArray();
        if (array.Length == 0)
            throw new ArgumentException("An error collection is empty on matching problem", nameof(errors));
        
        var firstError = array.First().Error;
        var isTheSameTypeError = array.All(error => error.Error.Type == firstError.Type);
        var aggregatedErrorMessageBuilder = new StringBuilder(array.Length);
        foreach (var error in array)
        {
            aggregatedErrorMessageBuilder.AppendLine(error.Error.Message);
        }
        var aggregatedErrorMessage = aggregatedErrorMessageBuilder.ToString().TrimEnd();
        var aggregatedError = isTheSameTypeError ? new DomainError(aggregatedErrorMessage, firstError.Type) : DomainError.Failure(aggregatedErrorMessage);

        return aggregatedError;
    }
}