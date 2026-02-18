namespace JournalChallenge.Infrastructure.Core.Abstractions.Behaviors;

using CSharpFunctionalExtensions;

using FluentValidation;
using FluentValidation.Results;

using JournalChallenge.Application.Core.Abstractions.ErrorHandling;
using JournalChallenge.Application.Core.Abstractions.Messaging;
using JournalChallenge.Infrastructure.Core.Implementations;

internal static class ValidationDecorator
{
    private static IError<DomainError> CreateValidationError(ValidationFailure[] validationFailures,
        IRestCustomResultsHandler resultsHandler)
    {
        var mapped = validationFailures.Select(x => DomainError.Validation(x.ErrorMessage)).ToArray();
        var validationError = resultsHandler.FoldErrors(mapped);

        return validationError;
    }

    private static async Task<ValidationFailure[]> ValidateAsync<TCommand>(
        TCommand command,
        IEnumerable<IValidator<TCommand>> validators)
    {
        var array = validators as IValidator<TCommand>[] ?? validators.ToArray();
        if (array.Length == 0)
            return [];

        var context = new ValidationContext<TCommand>(command);

        ValidationResult[] validationResults = await Task.WhenAll(
            array.Select(validator => validator.ValidateAsync(context)));

        ValidationFailure[] validationFailures = validationResults
                                                 .Where(validationResult => !validationResult.IsValid)
                                                 .SelectMany(validationResult => validationResult.Errors)
                                                 .ToArray();

        return validationFailures;
    }

    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        IEnumerable<IValidator<TCommand>> validators,
        IRestCustomResultsHandler resultsHandler)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse, IError<DomainError>>> HandleAsync(TCommand command,
            CancellationToken cancellationToken)
        {
            var validationFailures = await ValidateAsync(command, validators);

            if (validationFailures.Length == 0)
                return await innerHandler.HandleAsync(command, cancellationToken);

            var validationError = CreateValidationError(validationFailures, resultsHandler);

            return Result.Failure<TResponse, IError<DomainError>>(validationError);
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        IEnumerable<IValidator<TCommand>> validators,
        IRestCustomResultsHandler resultsHandler)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<UnitResult<IError<DomainError>>> HandleAsync(TCommand command, CancellationToken cancellationToken)
        {
            var validationFailures = await ValidateAsync(command, validators);

            if (validationFailures.Length == 0)
                return await innerHandler.HandleAsync(command, cancellationToken);

            var validationError = CreateValidationError(validationFailures, resultsHandler);

            return UnitResult.Failure(validationError);
        }
    }
}