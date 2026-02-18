namespace JournalChallenge.Application.Core.Abstractions.Messaging;

using CSharpFunctionalExtensions;

using JournalChallenge.Application.Core.Abstractions.ErrorHandling;

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse, IError<DomainError>>> HandleAsync(TQuery query, CancellationToken cancellationToken);
}