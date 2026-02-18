namespace JournalChallenge.Infrastructure.Core.Implementations;

using System.Text;

using CSharpFunctionalExtensions;

using JournalChallenge.Application.Core.Abstractions.ErrorHandling;
using JournalChallenge.Infrastructure.Core.Abstractions;


public interface IRestCustomResultsHandler: ICustomResultsHandler<DomainError, Exception>
{
}
// public interface IRestCustomResultsHandler: ICustomResultsHandler<DomainError, RpcException>
// {
// }

// internal sealed class CustomResultsHandler : IRestCustomResultsHandler
// {
//     public RpcException MatchProblem(IError<DomainError> error)
//     {
//         return error.Error.Type switch {
//             ErrorType.Validation => new RpcException(new Status(StatusCode.InvalidArgument, error.Error.Message)),
//             ErrorType.Problem => new RpcException(new Status(StatusCode.FailedPrecondition, error.Error.Message)),
//             ErrorType.NotFound => new RpcException(new Status(StatusCode.NotFound, error.Error.Message)),
//             ErrorType.Conflict => new RpcException(new Status(StatusCode.AlreadyExists, error.Error.Message)),
//             ErrorType.Unauthenticated => new RpcException(new Status(StatusCode.Unauthenticated, error.Error.Message)),
//             ErrorType.Failure => new RpcException(new Status(StatusCode.Internal, error.Error.Message)),
//             _ => new RpcException(new Status(StatusCode.Internal, "Server failure"))
//         };
//     }
//
//     public RpcException MatchProblem(IEnumerable<IError<DomainError>> errors)
//     {
//         var matchedErrors = FoldErrors(errors);
//         var matchedProblem = MatchProblem(matchedErrors);
//         
//         return matchedProblem;
//     }
//     
//     public IError<DomainError> FoldErrors(IEnumerable<IError<DomainError>> errors)
//     {
//         var array = errors as IError<DomainError>[] ?? errors.ToArray();
//         if (array.Length == 0)
//             throw new ArgumentException("An error collection is empty on matching problem", nameof(errors));
//         
//         var firstError = array.First().Error;
//         var isTheSameTypeError = array.All(error => error.Error.Type == firstError.Type);
//         var aggregatedErrorMessageBuilder = new StringBuilder(array.Length);
//         foreach (var error in array)
//         {
//             aggregatedErrorMessageBuilder.AppendLine(error.Error.Message);
//         }
//         var aggregatedErrorMessage = aggregatedErrorMessageBuilder.ToString();
//         var aggregatedError = isTheSameTypeError ? new DomainError(aggregatedErrorMessage, firstError.Type) : DomainError.Failure(aggregatedErrorMessage);
//
//         return aggregatedError;
//     }
// }