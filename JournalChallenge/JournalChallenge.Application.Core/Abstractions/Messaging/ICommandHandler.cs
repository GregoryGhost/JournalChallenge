namespace JournalChallenge.Application.Core.Abstractions.Messaging;

using JournalChallenge.Application.Core.Abstractions.ErrorHandling;

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task<UnitResult<IError<DomainError>>> HandleAsync(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse, IError<DomainError>>> HandleAsync(TCommand command, CancellationToken cancellationToken);
}
