namespace JournalChallenge.Infrastructure.Core.Abstractions;

using CSharpFunctionalExtensions;

using JournalChallenge.Application.Core.Abstractions.ErrorHandling;

public interface ICustomResultsHandler<in TError, out TOut>
{
    TOut MatchProblem(IError<TError> error);
    
    TOut MatchProblem(IEnumerable<IError<DomainError>> errors);
    
    IError<DomainError> FoldErrors(IEnumerable<IError<DomainError>> errors);
}